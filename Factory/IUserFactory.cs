using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public interface IUserFactory : IBaseFactory
    {
        public User CreateNamed(string username);
    }
}