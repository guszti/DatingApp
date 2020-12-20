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
        protected IUnitOfWork unitOfWork;

        protected IMapper mapper;
        
        public BaseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        protected async Task<ActionResult<T>> CreateAction<T, U>(T newResource, U resourceDto) where T : class, IEntity where U : class
        {
            if (!ModelState.IsValid || newResource == null || resourceDto == null)
            {
                return BadRequest();
            }
            
            this.mapper.Map(resourceDto, newResource);
            
            this.unitOfWork.BaseRepository.AddNew<T>(newResource);

            if (await this.unitOfWork.SaveChangesAsync())
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
            var result = await this.unitOfWork.BaseRepository.FindAll<T, U>(gridParamsDto);
            
            Response.AddPaginationHeader(result.Page, result.Limit, result.Total, result.TotalPages);
            
            return Ok(result);
        }

        protected async Task<ActionResult<U>> ShowAction<T, U>(int id) where T : class, IEntity where U : class
        {
            var resource = await this.unitOfWork.BaseRepository.FindById<T, U>(id);

            if (resource == null)
            {
                return NotFound();
            }
            
            return Ok(resource);
        }

        protected async Task<IActionResult> PutAction<T, U>(int id, T resourceDto) where U : class, IEntity
        {
            var resource = await this.unitOfWork.BaseRepository.FindById<U>(id);
        
            if (!ModelState.IsValid || resource == null || resourceDto == null)
            {
                return NotFound();
            }

            this.mapper.Map(resourceDto, resource);
            
            this.unitOfWork.BaseRepository.Update(resource);

            if (await this.unitOfWork.SaveChangesAsync())
            {
                return NoContent();
            }
            
            return BadRequest();
        }

        protected async Task<IActionResult> PatchAction<T, U>(int id, JsonPatchDocument<U> resourceDto)
            where T : class, IEntity where U : class
        {
            var resource = await this.unitOfWork.BaseRepository.FindById<T>(id);

            if (!ModelState.IsValid || resource == null || resourceDto == null)
            {
                return NotFound();
            }
            
            var tempDto = this.mapper.Map<U>(resource);

            resourceDto.ApplyTo(tempDto);

            this.mapper.Map(tempDto, resource);

            this.unitOfWork.BaseRepository.Update(resource);

            if (await this.unitOfWork.SaveChangesAsync())
            {
                return NoContent();
            }
            
            return BadRequest();
        }
        
        protected async Task<IActionResult> DeleteAction<T>(int id) where T : class, IEntity
        {
            var entity = await this.unitOfWork.BaseRepository.FindById<T>(id);

            if (entity == null)
            {
                return NotFound();
            }
            
            this.unitOfWork.BaseRepository.Remove(entity);

            if (await this.unitOfWork.SaveChangesAsync())
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}