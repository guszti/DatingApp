using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.SignalR
{
    public class PresenceTracker : IPresenceTrackerInterface
    {
        private static readonly Dictionary<int, List<string>> activeUsers = new Dictionary<int, List<string>>();

        public bool AddUserData(int id, string connectionId)
        {
            lock (activeUsers) {
                if (activeUsers.ContainsKey(id))
                {
                    activeUsers[id].Add(connectionId);
                }
                else
                {
                    activeUsers.Add(id, new List<string>{connectionId});

                    return true;
                }
            }

            return false;
        }

        public bool RemoveUserData(int id, string connectionId)
        {
            lock (activeUsers)
            {
                if (!activeUsers.ContainsKey(id)) return false;

                activeUsers[id].Remove(connectionId);

                if (activeUsers[id].Count == 0)
                {
                    activeUsers.Remove(id);

                    return true;
                }
            }

            return false;
        }

        public Task<int[]> GetActiveUsers()
        {
            int[] onlineUsers;
            
            lock (activeUsers)
            {
                onlineUsers = activeUsers.OrderBy(pair => pair.Key).Select(pair => pair.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public List<string> GetUserConnections(int id)
        {
            List<string> connectionIds;

            lock (activeUsers)
            {
                connectionIds = activeUsers.GetValueOrDefault(id);
            }

            return connectionIds;
        }
    }
}