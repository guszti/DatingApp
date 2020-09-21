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

        public async Task<IUser> Register(User user, string password)
        {
            this.authService.CreatePasswordHash(password, out var passwordHash, out var salt);

            user.Password = passwordHash;
            user.Salt = salt;

            await this.dataContext.User.AddAsync(user);
            await this.dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<IUser> Login(string username, string password)
        {
            IUser user = await this.dataContext.User.FirstOrDefaultAsync(o => o.Username == username);

            if (null == user) return null;

            if (!this.authService.ValidatePassword(password, user.Password, user.Salt)) return null;

            return user;
        }

        public async Task<bool> DoesUserExist(string username)
        {
           return await this.dataContext.User.AnyAsync(o => o.Username == username);
        }
    }
}