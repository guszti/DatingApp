using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
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

        public async Task<IEnumerable<UserWithPhotosDto>> FindAll()
        {
            return await this.context.User
                .ProjectTo<UserWithPhotosDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}