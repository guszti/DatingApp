using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public interface IMessageFactory : IBaseFactory
    {
        public Message CreateWithUsers(User source, User target, string content);
    }
}