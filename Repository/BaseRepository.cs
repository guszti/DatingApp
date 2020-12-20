using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class BaseRepository : IBaseRepository
    {
        protected DataContext context;

        protected IMapper mapper;
        
        public BaseRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<U> FindById<T, U>(int id) where T : class, IEntity where U : class
        {
            return await this.context.Set<T>()
                .Where(o => o.Id == id)
                .ProjectTo<U>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Grid<U>> FindAll<T, U>(GridParamsDto gridParamsDto) where T : class, IEntity where U : class
        {
            var query = this.context.Set<T>()
                .ProjectTo<U>(this.mapper.ConfigurationProvider)
                .AsNoTracking();

            return await Grid<U>.CreateGridAsync(query, gridParamsDto.Page, gridParamsDto.Limit);
        }

        public async Task<T> FindById<T>(int id) where T : class, IEntity
        {
            return await this.context.Set<T>()
                .Where(o => o.Id == id)
                .SingleOrDefaultAsync();
        }

        public void Remove<T>(T entity)
        {
            this.context.Remove(entity);
        }

        public void Update<T>(T entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }
        

        public async void AddNew<T>(T entity) where T : class
        {
            await this.context.Set<T>()
                .AddAsync(entity);
        }
    }
}