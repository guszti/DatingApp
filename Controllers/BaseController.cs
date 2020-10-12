using System.Collections.Generic;
using System.Threading.Tasks;
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
        protected IBaseRepository baseRepositoryInterface;
        
        public BaseController(IBaseRepository baseRepositoryInterface)
        {
            this.baseRepositoryInterface = baseRepositoryInterface;
        }
        
        public async Task<ActionResult<IEnumerable<T>>> IndexAction<T, U>() where T : class, IEntity
        {
            return Ok(await this.baseRepositoryInterface.FindAll<T, U>());
        }

        public async Task<ActionResult<U>> ShowAction<T, U>(int id) where T : class, IEntity where U : class
        {
            var resource = await this.baseRepositoryInterface.FindById<T, U>(id);

            if (resource == null)
            {
                return NotFound();
            }
            
            return Ok(resource);
        }

        // public async Task<IActionResult> PutAction<T, U>(T resourceDto) where T : class
        // {
        //     
        // }
        
        public async Task<IActionResult> DeleteAction<T>(int id) where T : class, IEntity
        {
            var entity = this.baseRepositoryInterface.FindById<T>(id);

            if (entity == null)
            {
                return NotFound();
            }
            
            this.baseRepositoryInterface.Remove(entity);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}