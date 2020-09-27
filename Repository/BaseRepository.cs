using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class BaseRepository : IBaseRepository
    {
        protected DataContext context;
        
        public BaseRepository(DataContext context)
        {
            this.context = context;
        }

        public virtual async Task<T> FindById<T>(int id) where T : class, IEntity
        {
            return await this.context.Set<T>()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public virtual async Task<IEnumerable<T>> FindAll<T>() where T : class, IEntity
        {
            return await this.context.Set<T>().ToListAsync();
        }
        
        public async Task<bool> SaveAll()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}