using System.Linq;
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

        private IAuthService authService;

        public AuthRepository(DataContext dataContext, IAuthService authService)
        {
            this.dataContext = dataContext;
            this.authService = authService;
        }

        public async Task<User> Register(User user, string password)
        {
            this.authService.CreatePasswordHash(password, out var passwordHash, out var salt);

            user.Password = passwordHash;
            user.Salt = salt;

            await this.dataContext.Users.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            User user = await this.dataContext.Users.FirstOrDefaultAsync(o => o.Username == username);

            if (null == user) return null;

            if (!this.authService.ValidatePassword(password, user.Password, user.Salt)) return null;

            return user;
        }

        public async Task<bool> DoesUserExist(string username)
        {
           return await this.dataContext.Users.AnyAsync(o => o.Username == username);
        }
    }
}