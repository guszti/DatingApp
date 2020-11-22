using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Enum;
using DatingApp.API.Helpers;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<User> FindById(int id)
        {
            return await this.context.User
                .Where(o => o.Id == id)
                .Include(user => user.Photos)
                .SingleOrDefaultAsync();
        }

        public async Task<Grid<UserWithPhotosDto>> FindAll(UserParamsDto userParams)
        {
            var query = this.context.User.AsQueryable();

            if (userParams.Gender.GetType() == typeof(Gender))
            {
                query = query.Where(o => o.Gender == userParams.Gender).AsNoTracking();
            }
            
            return await Grid<UserWithPhotosDto>.CreateGridAsync(
                query.ProjectTo<UserWithPhotosDto>(this.mapper.ConfigurationProvider).AsNoTracking(),
                userParams.Page,
                userParams.Limit
            );
        }
    }
}