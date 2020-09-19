using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public interface IUserFactory
    {
        public User CreateNamed(string username);
    }
}