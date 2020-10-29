using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
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
    public class BaseController : ControllerBase
    {
        private IBaseRepository baseRepositoryInterface;

        protected IMapper mapper;
        
        public BaseController(IBaseRepository baseRepositoryInterface, IMapper mapper)
        {
            this.baseRepositoryInterface = baseRepositoryInterface;
            this.mapper = mapper;
        }

        protected async Task<ActionResult<T>> CreateAction<T, U>(T newResource, U resourceDto) where T : class, IEntity where U : class
        {
            if (newResource == null || resourceDto == null)
            {
                return BadRequest();
            }
            
            this.baseRepositoryInterface.AddNew<T, U>(newResource, resourceDto);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return Created("", new
                {
                    resource = newResource
                });
            }

            return BadRequest();
        }
        
        protected async Task<ActionResult<IEnumerable<U>>> IndexAction<T, U>() where T : class, IEntity
        {
            return Ok(await this.baseRepositoryInterface.FindAll<T, U>());
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
        
            if (resource == null)
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

            if (resource == null)
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
                return NoContent();
            }

            return BadRequest();
        }
    }
}