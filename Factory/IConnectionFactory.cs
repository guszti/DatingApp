using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public interface IConnectionFactory : IBaseFactory
    {
        public Connection CreateForGroup(Group group);
    }
}