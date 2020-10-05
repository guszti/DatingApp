using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected DataContext context;

        protected IBaseRepository baseRepositoryInterface;

        protected IMapper mapperInterface;
        
        public BaseController(DataContext context, IBaseRepository baseRepositoryInterface, IMapper mapperInterface)
        {
            this.context = context;
            this.baseRepositoryInterface = baseRepositoryInterface;
            this.mapperInterface = mapperInterface;
        }
        
        public async Task<ActionResult<IEnumerable<T>>> IndexAction<T>() where T : class, IEntity
        {
            return Ok(await this.baseRepositoryInterface.FindAll<T>());
        }

        public async Task<ActionResult<T>> ShowAction<T>(int id) where T : class, IEntity
        {
            return await this.baseRepositoryInterface.FindById<T>(id);
        }

        public async Task<IActionResult> DeleteAction<T>(T entity) where T : class
        {
            this.context.Remove(entity);

            await this.context.SaveChangesAsync();

            return NoContent();
        }
    }
}