using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext dataContext;
        
        public AuthRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IUser> Register(User user)
        {
            await this.dataContext.User.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await this.dataContext.User
                .Include(item => item.Photos)
                .FirstOrDefaultAsync(o => o.UserName == username);
            
            return user;
        }

        public async Task<bool> DoesUserExist(string username)
        {
           return await this.dataContext.User.AnyAsync(o => o.UserName == username);
        }
    }
}