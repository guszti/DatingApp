using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class DataSeed
    {
        public static async Task SeedUserData(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var fixtureJsonContent = System.IO.File.ReadAllText("assets/user_fixtures.json");
            var users = JsonConvert.DeserializeObject<List<User>>(fixtureJsonContent);

            var roles = new List<Role>
            {
                new Role {Name = IRole.Member},
                new Role {Name = IRole.Moderator},
                new Role {Name = IRole.Admin}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, IRole.Member);
            }

            var admin = new User
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {IRole.Admin, IRole.Moderator});
        }
    }
}