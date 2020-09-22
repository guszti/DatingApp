using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public abstract class BaseRepository : IBaseRepository
    {
        private DataContext context;
        
        protected BaseRepository(DataContext context)
        {
            this.context = context;
        }

        public void AddNew<T>(T entity) where T : class
        {
            this.context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this.context.Remove(entity);
        }

        public async Task<T> FindById<T>(int id) where T : class, IEntity
        {
            return await this.context.Set<T>().FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<T>> FindAll<T>() where T : class, IEntity
        {
            return await this.context.Set<T>().ToListAsync();
        }
        
        public async Task<bool> SaveAll()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}