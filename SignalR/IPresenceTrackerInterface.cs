using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.API.SignalR
{
    public interface IPresenceTrackerInterface
    {
        public bool AddUserData(int id, string connectionId);

        public bool RemoveUserData(int id, string connectionId);

        public Task<int[]> GetActiveUsers();

        public List<string> GetUserConnections(int id);
    }
}