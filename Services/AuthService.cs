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

        public bool ValidatePassword(string password, byte[] passwordHash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                byte[] hashedPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashedPassword.Length; i++)
                {
                    if (hashedPassword[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }
    }
}