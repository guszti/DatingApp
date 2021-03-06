using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Model
{
    public class UserRole : IdentityUserRole<int>, IUserRole
    {
        public int Id { get; set; }
        public User User { get; set; }
        
        public Role Role { get; set; }
    }
}