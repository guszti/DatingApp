using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.Services;

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

        public Task<User> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DoesUserExist(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}