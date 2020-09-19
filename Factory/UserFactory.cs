using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class UserFactory : BaseFactory, IUserFactory
    {
        public User CreateNamed(string username)
        {
            User user = this.CreateNew<User>();

            user.Username = username;

            return user;
        }
    }
}