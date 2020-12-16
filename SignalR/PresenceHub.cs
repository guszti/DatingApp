using System;
using System.Threading.Tasks;
using DatingApp.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker presenceTracker;
        
        public PresenceHub(PresenceTracker presenceTracker)
        {
            this.presenceTracker = presenceTracker;
        }
        
        public async override Task OnConnectedAsync()
        {
            await this.presenceTracker.AddUserData(Context.User.GetUserId(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserId());

            var currentUsers = await this.presenceTracker.GetActiveUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await this.presenceTracker.RemoveUserData(Context.User.GetUserId(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserId());

            var currentUsers = await this.presenceTracker.GetActiveUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}