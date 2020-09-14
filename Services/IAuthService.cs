using DatingApp.API.Model;

namespace DatingApp.API.Services
{
    public interface IAuthService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt);

        public bool ValidatePassword(string password, byte[] passwordHash, byte[] salt);

        public string CreateJwtToken(string username, int userId);
    }
}