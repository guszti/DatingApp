using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class UserFactory : IUserFactory
    {
        public User CreateNew()
        {
            return new User();
        }

        public User CreateNamed(string username)
        {
            User user = CreateNew();

            user.Username = username;

            return user;
        }
    }
}