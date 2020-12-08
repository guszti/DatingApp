using DatingApp.API.Model;

namespace DatingApp.API.Services
{
    public interface IAuthService
    {
        public string CreateJwtToken(string username, int userId);
    }
}