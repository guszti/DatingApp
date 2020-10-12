using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Repository
{
    public interface IBaseRepository
    {
        public Task<U> FindById<T, U>(int id) where T : class, IEntity where U : class;

        public Task<T> FindById<T>(int id) where T : class, IEntity;
        
        public Task<IEnumerable<U>> FindAll<T, U>() where T : class, IEntity;

        public void Remove<T>(T entity);

        public Task<bool> SaveAll();
    }
}