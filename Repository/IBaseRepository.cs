using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IBaseRepository
    {
        public Task<U> FindById<T, U>(int id) where T : class, IEntity where U : class;

        public Task<T> FindById<T>(int id) where T : class, IEntity;
        
        public Task<IEnumerable<U>> FindAll<T, U>() where T : class, IEntity;

        public void Remove<T>(T entity);

        public void Update<T>(T entity);        

        public void AddNew<T, U>(T entity, U entityDto) where T : class, IEntity where U : class;
        
        public Task<bool> SaveAll();
    }
}