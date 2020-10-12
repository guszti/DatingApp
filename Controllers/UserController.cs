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
            IBaseRepository baseRepositoryInterface,
            IUserRepository userRepositoryInterface
            ) : base(baseRepositoryInterface)
        {
            this.userRepositoryInterface = userRepositoryInterface;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithPhotosDto>>> Index()
        {
            return Ok(await this.userRepositoryInterface.FindAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithPhotosDto>> Show(int id)
        {
            return await this.userRepositoryInterface.FindById(id);
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