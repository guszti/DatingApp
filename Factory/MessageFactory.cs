using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class MessageFactory : BaseFactory, IMessageFactory
    {
        public Message CreateWithUsers(IUser source, IUser target)
        {
            var message = this.CreateNew<Message>();

            message.SourceId = source.Id;
            message.TargetId = target.Id;

            return message;
        }
    }
}