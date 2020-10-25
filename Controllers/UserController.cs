using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UsersController : BaseController
    {
        private IUserFactory userFactory;
        
        public UsersController(IBaseRepository baseRepositoryInterface, IUserFactory userFactory) : base(baseRepositoryInterface)
        {
            this.userFactory = userFactory;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithPhotosDto>>> Index()
        {
            return await this.IndexAction<User, UserWithPhotosDto>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithPhotosDto>> Show(int id)
        {
            return await this.ShowAction<User, UserWithPhotosDto>(id);
        }

        [HttpPost("/")]
        public async Task<ActionResult<User>> Create(UserCreateDto user)
        {
            var newUser = this.userFactory.CreateNew<User>();
            
            return await this.CreateAction<User, UserCreateDto>(newUser, user);
        }

        [HttpPut("/{id}")]
        [HttpPatch("/{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto data)
        {
            return await this.UpdateAction<UserUpdateDto, User>(id, data);
        }

        [HttpDelete("/id")]
        public async Task<IActionResult> Delete(int id)
        {
            return await this.DeleteAction<User>(id);
        }
    }
}