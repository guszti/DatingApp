using System.Collections.Generic;

namespace DatingApp.API.Model
{
    public interface IRole
    {
        public ICollection<UserRole> Users { get; set; }
    }
}