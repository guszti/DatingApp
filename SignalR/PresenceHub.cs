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
        private const string UserIsOnline = "UserIsOnline";
        private const string GetOnlineUsers = "GetOnlineUsers";
        private const string UserIsOffline = "UserIsOffline";

        private readonly PresenceTracker presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            this.presenceTracker = presenceTracker;
        }

        public async override Task OnConnectedAsync()
        {
            var isOnline = this.presenceTracker.AddUserData(Context.User.GetUserId(), Context.ConnectionId);

            if (isOnline)
            {
                await Clients.Others.SendAsync(UserIsOnline, Context.User.GetUserId());
            }

            var currentUsers = await this.presenceTracker.GetActiveUsers();
            await Clients.Caller.SendAsync(GetOnlineUsers, currentUsers);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var isOffline = this.presenceTracker.RemoveUserData(Context.User.GetUserId(), Context.ConnectionId);

            if (isOffline)
            {
                await Clients.Others.SendAsync(UserIsOffline, Context.User.GetUserId());
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}