using System.Collections.Generic;

namespace DatingApp.API.Model
{
    public interface IGroup
    {
        string Name { get; set; }
        
        ICollection<Connection> Connections { get; set; }
    }
}