using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public interface IGroupFactory : IBaseFactory
    {
        public Group CreateNamed(string name);
    }
}