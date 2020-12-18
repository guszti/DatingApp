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
    }
}