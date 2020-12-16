using System.Threading.Tasks;

namespace DatingApp.API.SignalR
{
    public interface IPresenceTrackerInterface
    {
        public Task AddUserData(int id, string connectionId);

        public Task RemoveUserData(int id, string connectionId);

        public Task<int[]> GetActiveUsers();
    }
}