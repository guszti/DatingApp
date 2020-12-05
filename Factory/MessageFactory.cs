using DatingApp.API.Dtos;
using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class MessageFactory : BaseFactory, IMessageFactory
    {
        public Message CreateWithUsers(User source, User target, string content)
        {
            var message = this.CreateNew<Message>();

            message.Source = source;
            message.Target = target;
            message.Content = content;

            return message;
        }
    }
}