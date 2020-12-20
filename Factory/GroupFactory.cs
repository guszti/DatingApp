using System.Collections.Generic;
using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class GroupFactory : BaseFactory, IGroupFactory
    {
        public Group CreateNamed(string name)
        {
            var group = this.CreateNew<Group>();

            group.Name = name;
            group.Connections = new List<Connection>();

            return group;
        }
    }
}