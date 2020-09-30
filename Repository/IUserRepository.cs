using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;

namespace DatingApp.API.Repository
{
    public interface IUserRepository : IBaseRepository
    {
        public Task<UserWithPhotosDto> FindById(int id);

        public Task<IEnumerable<UserWithPhotosDto>> FindAll();
    }
}