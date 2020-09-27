using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<IUser> FindById(int id)
        {
            return await this.context.User
                .Include(o => o.Photos)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<IUser>> FindAll()
        {
            return await this.context.User
                .Include(o => o.Photos)
                .ToListAsync();
        }
    }
}