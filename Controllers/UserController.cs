using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UsersController : BaseController
    {
        private IUserFactory userFactory;
        
        public UsersController(
            IBaseRepository baseRepositoryInterface, 
            IMapper mapper, 
            IUserFactory userFactory) : base(baseRepositoryInterface, mapper)
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
        
        [HttpPost]
        public async Task<ActionResult<User>> Create(UserCreateDto user)
        {
            var newUser = this.userFactory.CreateNew<User>();
            
            return await this.CreateAction<User, UserCreateDto>(newUser, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserUpdateDto data)
        {
            return await this.PutAction<UserUpdateDto, User>(id, data);
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<UserUpdateDto> data)
        {
            return await this.PatchAction<User, UserUpdateDto>(id, data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await this.DeleteAction<User>(id);
        }
    }
}