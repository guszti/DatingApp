using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        public GroupRepository(DataContext context, IMapper mapper)
            : base(context, mapper)
        {
            
        }

        public async Task<Group> GetMessageGroup(string name)
        {
            return await this.context.Group
                .Include(g => g.Connections)
                .FirstOrDefaultAsync(g => g.Name == name);
        }

        public Task<Group> GetGroupForConnection(string connectionId)
        {
            return this.context.Group
                .Include(g => g.Connections)
                .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }
    }
}