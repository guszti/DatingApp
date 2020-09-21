using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DatingApp.API.Model;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class DataSeed
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        
        public static void SeedUserData(DataContext context)
        {
            if (!context.User.Any())
            {
                var fixtureJsonContent = System.IO.File.ReadAllText("assets/user_fixtures.json");
                var users = JsonConvert.DeserializeObject<List<User>>(fixtureJsonContent);

                foreach (var user in users)
                {
                    CreatePasswordHash("password", out byte[] passwordHash, out byte[] salt);

                    user.Password = passwordHash;
                    user.Salt = salt;
                    user.Username = user.Username.ToLower();

                    context.User.Add(user);
                }

                context.SaveChanges();
            }
        }
    }
}