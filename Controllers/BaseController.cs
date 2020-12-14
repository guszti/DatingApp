using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Helpers;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(UpdateUserLastActive))]
    public class BaseController : ControllerBase
    {
        protected IBaseRepository baseRepositoryInterface;

        protected IMapper mapper;
        
        public BaseController(IBaseRepository baseRepositoryInterface, IMapper mapper)
        {
            this.baseRepositoryInterface = baseRepositoryInterface;
            this.mapper = mapper;
        }

        protected async Task<ActionResult<T>> CreateAction<T, U>(T newResource, U resourceDto) where T : class, IEntity where U : class
        {
            if (!ModelState.IsValid || newResource == null || resourceDto == null)
            {
                return BadRequest();
            }
            
            this.mapper.Map(resourceDto, newResource);
            
            this.baseRepositoryInterface.AddNew<T>(newResource);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return Created("", new
                {
                    resource = newResource
                });
            }

            return BadRequest();
        }
        
        protected async Task<ActionResult<Grid<U>>> IndexAction<T, U>(GridParamsDto gridParamsDto) where T : class, IEntity where U : class
        {
            var result = await this.baseRepositoryInterface.FindAll<T, U>(gridParamsDto);
            
            Response.AddPaginationHeader(result.Page, result.Limit, result.Total, result.TotalPages);
            
            return Ok(result);
        }

        protected async Task<ActionResult<U>> ShowAction<T, U>(int id) where T : class, IEntity where U : class
        {
            var resource = await this.baseRepositoryInterface.FindById<T, U>(id);

            if (resource == null)
            {
                return NotFound();
            }
            
            return Ok(resource);
        }

        protected async Task<IActionResult> PutAction<T, U>(int id, T resourceDto) where U : class, IEntity
        {
            var resource = await this.baseRepositoryInterface.FindById<U>(id);
        
            if (!ModelState.IsValid || resource == null || resourceDto == null)
            {
                return NotFound();
            }

            this.mapper.Map(resourceDto, resource);
            
            this.baseRepositoryInterface.Update(resource);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return NoContent();
            }
            
            return BadRequest();
        }

        protected async Task<IActionResult> PatchAction<T, U>(int id, JsonPatchDocument<U> resourceDto)
            where T : class, IEntity where U : class
        {
            var resource = await this.baseRepositoryInterface.FindById<T>(id);

            if (!ModelState.IsValid || resource == null || resourceDto == null)
            {
                return NotFound();
            }
            
            var tempDto = this.mapper.Map<U>(resource);

            resourceDto.ApplyTo(tempDto);

            this.mapper.Map(tempDto, resource);

            this.baseRepositoryInterface.Update(resource);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return NoContent();
            }
            
            return BadRequest();
        }
        
        protected async Task<IActionResult> DeleteAction<T>(int id) where T : class, IEntity
        {
            var entity = await this.baseRepositoryInterface.FindById<T>(id);

            if (entity == null)
            {
                return NotFound();
            }
            
            this.baseRepositoryInterface.Remove(entity);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}