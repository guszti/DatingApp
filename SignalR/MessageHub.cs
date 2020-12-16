using System;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.API.SignalR
{
    public class MessageHub : Hub
    {
        private const string ReceiveMessageThread = "ReceiveMessageThread";
        
        private IMessageRepository messageRepository;

        private IMapper mapper;

        private IUserRepository userRepository;
        
        public MessageHub(IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository)
        {
            this.messageRepository = messageRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var targetUsername = httpContext.Request.Query["user"].ToString();
            var groupName = this.MakeGroupName(this.Context.User.GetUsername(), targetUsername);
            var otherUser = this.userRepository.FindByUsername(targetUsername);
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await this.messageRepository
                .GetMessageThread(Context.User.GetUserId(), otherUser.Id);

            await Clients.Group(groupName).SendAsync(ReceiveMessageThread, messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        private string MakeGroupName(string source, string target)
        {
            var comparisonResult = string.CompareOrdinal(source, target) < 0;

            return comparisonResult ? $"{source}-{target}" : $"{target}-{source}";
        }
    }
}