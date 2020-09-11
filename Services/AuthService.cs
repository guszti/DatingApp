using System.Security.Cryptography;
using DatingApp.API.Model;

namespace DatingApp.API.Services
{
    public class AuthService : IAuthService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }            
        }
    }
}