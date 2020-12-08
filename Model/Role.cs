using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Model
{
    public class Role : IdentityRole<int>, IRole
    {
        public ICollection<UserRole> Users { get; set; }
    }
}