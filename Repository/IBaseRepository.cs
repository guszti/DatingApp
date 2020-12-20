using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IBaseRepository
    {
        public Task<U> FindById<T, U>(int id) where T : class, IEntity where U : class;

        public Task<T> FindById<T>(int id) where T : class, IEntity;
        
        public Task<Grid<U>> FindAll<T, U>(GridParamsDto gridParamsDto) where T : class, IEntity where U : class;

        public void Remove<T>(T entity);

        public void Update<T>(T entity);        

        public void AddNew<T>(T entity) where T : class;
    }
}