using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;

namespace DatingApp.API.Repository
{
    public interface IMessageRepository : IBaseRepository
    {
        Task<Grid<MessageDto>> GetMessagesForUser();

        Task<IEnumerable<MessageDto>> GetMessageThread(int sourceId, int targetId);
    }
}