using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IGroupRepository : IBaseRepository
    {
        Task<Group> GetMessageGroup(string name);

        Task<Group> GetGroupForConnection(string connectionId);
    }
}