using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IUserRepository : IBaseRepository
    {
        public Task<User> FindById(int id);

        public Task<Grid<UserWithPhotosDto>> FindAll(UserParamsDto userParams);
    }
}