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

        private IMessageRepository messageRepository;

        private IMapper mapper;

        private IUserRepository userRepository;

        private IMessageFactory messageFactory;

        private IGroupFactory groupFactory;

        private IConnectionFactory connectionFactory;

        private IGroupRepository groupRepository;

        private IConnectionRepository connectionRepository;

        public MessageHub(
            IMessageRepository messageRepository,
            IMapper mapper,
            UserRepository userRepository,
            IMessageFactory messageFactory,
            IGroupFactory groupFactory,
            IConnectionFactory connectionFactory,
            IGroupRepository groupRepository,
            IConnectionRepository connectionRepository
        )
        {
            this.messageRepository = messageRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.messageFactory = messageFactory;
            this.groupFactory = groupFactory;
            this.connectionFactory = connectionFactory;
            this.groupRepository = groupRepository;
            this.connectionRepository = connectionRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var targetUsername = httpContext.Request.Query["user"].ToString();
            var groupName = this.MakeGroupName(this.Context.User.GetUsername(), targetUsername);
            var otherUser = this.userRepository.FindByUsername(targetUsername);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await this.AddToGroup(Context, groupName);

            var messages = await this.messageRepository
                .GetMessageThread(Context.User.GetUserId(), otherUser.Id);

            await Clients.Group(groupName).SendAsync(ReceiveMessageThread, messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await this.RemoveFromGroup(Context.ConnectionId);
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

            var sourceUser = await this.userRepository.FindById(sourceId);
            var targetUser = await this.userRepository.FindById(createMessageDto.TargetUserId);

            if (sourceUser == null || targetUser == null)
            {
                throw new HubException("Source or target user not found.");
            }

            var message = this.messageFactory.CreateWithUsers(sourceUser, targetUser, createMessageDto.Content);

            var groupName = this.MakeGroupName(sourceUser.UserName, targetUser.UserName);
            var group = await this.groupRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == targetUser.UserName))
            {
                message.SeenAt = DateTime.UtcNow;
            }
            
            this.messageRepository.AddNew(message);

            if (await this.messageRepository.SaveAll())
            {
                await Clients.Group(groupName).SendAsync(NewMessage, this.mapper.Map<MessageDto>(message));
            }

            throw new HubException("Something went wrong.");
        }

        public async Task<bool> AddToGroup(HubCallerContext context, string name)
        {
            var group = await this.groupRepository.GetMessageGroup(name);

            if (group == null)
            {
                this.groupRepository.AddNew<Group>(this.groupFactory.CreateNamed(name));
            }

            var connection = this.connectionFactory.CreateForGroup(group);
            connection.Username = context.User.GetUsername();

            return await this.groupRepository.SaveAll();
        }

        public async Task RemoveFromGroup(string connectionId)
        {
            var connection = this.connectionRepository.FindByConnectionId(connectionId);
            this.connectionRepository.Remove(connection);

            await this.connectionRepository.SaveAll();
        }
    }
}