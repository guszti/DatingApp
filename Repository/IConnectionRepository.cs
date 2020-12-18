using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IConnectionRepository : IBaseRepository
    {
        Task<Connection> FindByConnectionId(string connectionId);
    }
}