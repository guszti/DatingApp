using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Model;
using DatingApp.API.ParamObject;

namespace DatingApp.API.Repository
{
    public interface IMessageRepository : IBaseRepository
    {
        Task<Grid<MessageDto>> GetMessagesForUser(MessageParams messageParams);

        Task<IEnumerable<MessageDto>> GetMessageThread(int sourceId, int targetId);
    }
}