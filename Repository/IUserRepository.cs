using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IUserRepository : IBaseRepository
    {
        public Task<IUser> FindById(int id);

        public Task<IEnumerable<IUser>> FindAll();
    }
}