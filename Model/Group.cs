using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Model
{
    public class Group : IGroup
    {
        [Key]
        public string Name { get; set; }
        
        public ICollection<Connection> Connections { get; set; }
    }
}