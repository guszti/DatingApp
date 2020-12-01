using AutoMapper;
using DatingApp.API.Data;

namespace DatingApp.API.Repository
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}