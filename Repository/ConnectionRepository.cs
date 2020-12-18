using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class ConnectionRepository : BaseRepository, IConnectionRepository
    {
        public ConnectionRepository(DataContext context, IMapper mapper)
            : base(context, mapper)
        {
            
        }
        
        public async Task<Connection> FindByConnectionId(string connectionId)
        {
            return await this.context.Connection.FirstOrDefaultAsync(c => c.ConnectionId == connectionId);
        }
    }
}