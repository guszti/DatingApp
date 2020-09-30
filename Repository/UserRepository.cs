using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private IMapper mapper;
        
        public UserRepository(DataContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public async Task<UserWithPhotosDto> FindById(int id)
        {
            return await this.context.User
                .Where(o => o.Id == id)
                .ProjectTo<UserWithPhotosDto>(this.mapper.ConfigurationProvider)
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