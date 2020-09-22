using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IBaseRepository
    {
        public void AddNew<T>(T entity) where T : class;

        public void Delete<T>(T entity) where T : class;

        public Task<T> FindById<T>(int id) where T : class, IEntity;

        public Task<IEnumerable<T>> FindAll<T>() where T : class, IEntity;
        
        public Task<bool> SaveAll();
    }
}