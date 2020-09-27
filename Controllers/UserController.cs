using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UserController : BaseController
    {
        private IUserRepository userRepositoryInterface;
        
        public UserController(DataContext context, IUserRepository userRepositoryInterface) : base(context)
        {
            this.userRepositoryInterface = userRepositoryInterface;
        }
        
        [HttpGet("/")]
        public async Task<ActionResult<IEnumerable<IUser>>> Index()
        {
             return Ok(await this.userRepositoryInterface.FindAll());
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<User>> Show()
        {
            return await this.ShowAction<User>(1);
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