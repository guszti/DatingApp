using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Extensions;
using DatingApp.API.Factory;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.API.SignalR
{
    public class MessageHub : Hub
    {
        private const string ReceiveMessageThread = "ReceiveMessageThread";
        private const string NewMessage = "NewMessage";
        private const string NewMessageReceived = "NewMessageReceived";
        private const string UpdatedGroup = "UpdatedGroup";

        private IUnitOfWork unitOfWork;

        private IMapper mapper;

        private IMessageFactory messageFactory;

        private IGroupFactory groupFactory;

        private IConnectionFactory connectionFactory;

        private PresenceTracker presenceTracker;

        private IHubContext<PresenceHub> presenceHub;

        public MessageHub(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMessageFactory messageFactory,
            IGroupFactory groupFactory,
            IConnectionFactory connectionFactory,
            PresenceTracker presenceTracker,
            IHubContext<PresenceHub> presenceHub
        )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.messageFactory = messageFactory;
            this.groupFactory = groupFactory;
            this.connectionFactory = connectionFactory;
            this.presenceTracker = presenceTracker;
            this.presenceHub = presenceHub;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            Int32.TryParse(httpContext.Request.Query["user"], out int targetUserId);
            var targetUser = await this.unitOfWork.UserRepository.FindById(targetUserId);
            var groupName = this.MakeGroupName(this.Context.User.GetUsername(), targetUser.UserName);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group = await this.AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync(UpdatedGroup, group);

            var messages = await this.unitOfWork
                .MessageRepository
                .GetMessageThread(Context.User.GetUserId(), targetUserId);

            if (this.unitOfWork.HasChanges())
            {
                await this.unitOfWork.SaveChangesAsync();
            }

            await Clients.Caller.SendAsync(ReceiveMessageThread, messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await this.RemoveFromGroup();

            await Clients.Group(group.Name).SendAsync(UpdatedGroup, group);
            await base.OnDisconnectedAsync(exception);
        }

        private string MakeGroupName(string source, string target)
        {
            var comparisonResult = string.CompareOrdinal(source, target) < 0;

            return comparisonResult ? $"{source}-{target}" : $"{target}-{source}";
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            int sourceId = this.Context.User.GetUserId();

            if (sourceId == createMessageDto.TargetUserId)
            {
                throw new HubException("You cannot send messages to yourself.");
            }

            var sourceUser = await this.unitOfWork.UserRepository.FindById(sourceId);
            var targetUser = await this.unitOfWork.UserRepository.FindById(createMessageDto.TargetUserId);

            if (sourceUser == null || targetUser == null)
            {
                throw new HubException("Source or target user not found.");
            }

            var message = this.messageFactory.CreateWithUsers(sourceUser, targetUser, createMessageDto.Content);

            var groupName = this.MakeGroupName(sourceUser.UserName, targetUser.UserName);
            var group = await this.unitOfWork.GroupRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == targetUser.UserName))
            {
                message.SeenAt = DateTime.UtcNow;
            }
            else
            {
                var connectionIds = this.presenceTracker.GetUserConnections(targetUser.Id);

                if (connectionIds.Any())
                {
                    await this.presenceHub.Clients.Clients(connectionIds).SendAsync(NewMessageReceived,
                        new {username = sourceUser.UserName, knownAs = sourceUser.KnownAs});
                }
            }

            this.unitOfWork.MessageRepository.AddNew(message);
            
            if (await this.unitOfWork.SaveChangesAsync())
            {
                await Clients.Group(groupName).SendAsync(NewMessage, this.mapper.Map<MessageDto>(message));
            }
        }

        public async Task<Group> AddToGroup(string name)
        {
            var group = await this.unitOfWork.GroupRepository.GetMessageGroup(name);

            if (group == null)
            {
                group = this.groupFactory.CreateNamed(name);
                this.unitOfWork.GroupRepository.AddNew<Group>(group);
            }

            var connection = this.connectionFactory.CreateForGroup(group, this.Context.ConnectionId);
            connection.Username = Context.User.GetUsername();

            if (await this.unitOfWork.SaveChangesAsync()) return group;

            throw new HubException("Failed to add user to group.");
        }

        public async Task<Group> RemoveFromGroup()
        {
            var group = await this.unitOfWork.GroupRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            this.unitOfWork.ConnectionRepository.Remove(connection);

            if (await this.unitOfWork.SaveChangesAsync())
            {
                return group;
            }

            throw new HubException("Failed to remove from group.");
        }
    }
}