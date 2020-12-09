using System.Collections.Generic;

namespace DatingApp.API.Model
{
    public interface IRole
    {
        public const string Member = "Member";
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        
        public ICollection<UserRole> Users { get; set; }
    }
}