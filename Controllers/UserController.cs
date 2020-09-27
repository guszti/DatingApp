using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UsersController : BaseController
    {
        private IUserRepository userRepositoryInterface;
        
        public UsersController(
            DataContext context,
            IBaseRepository baseRepositoryInterface,
            IMapper mapperInterface,
            IUserRepository userRepositoryInterface
            ) : base(context, baseRepositoryInterface, mapperInterface)
        {
            this.userRepositoryInterface = userRepositoryInterface;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithPhotosDto>>> Index()
        {
            var users = await this.userRepositoryInterface.FindAll(); 
            
            return Ok(this.mapperInterface.Map<IEnumerable<UserWithPhotosDto>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithPhotosDto>> Show(int id)
        {
            var user = await this.userRepositoryInterface.FindById(id);
            return this.mapperInterface.Map<UserWithPhotosDto>(user);
        }
        
        /*[HttpPost("/")]
        public Task<IActionResult> Create()
        {
            
        }*/

        /*[HttpPatch("/{id}")]
        public Task<IActionResult> Update()
        {
            
        }*/

        /*[HttpDelete("/id")]
        public Task<IActionResult> Delete()
        {
            
        }*/
    }
}