using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IAuthRepository
    {
        public Task<IUser> Register(User user);

        public Task<User> Login(string username, string password);

        public Task<bool> DoesUserExist(string username);
    }
}