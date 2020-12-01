using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public interface IMessageFactory : IBaseFactory
    {
        public Message CreateWithUsers(IUser source, IUser target);
    }
}