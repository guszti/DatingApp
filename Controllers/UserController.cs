using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Extensions;
using DatingApp.API.Factory;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UsersController : BaseController
    {
        private IUserFactory userFactory;

        private IPhotoHandlerService photoHandlerService;

        private IPhotoFactory photoFactory;

        private IUserRepository userRepository;
        
        public UsersController(
            IBaseRepository baseRepositoryInterface, 
            IMapper mapper, 
            IUserFactory userFactory,
            IPhotoHandlerService photoHandlerService,
            IPhotoFactory photoFactory,
            IUserRepository userRepository
            
            ) : base(baseRepositoryInterface, mapper)
        {
            this.userFactory = userFactory;
            this.photoHandlerService = photoHandlerService;
            this.photoFactory = photoFactory;
            this.userRepository = userRepository;
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
            // TODO update logged in user only, get user -> User.GetUsername()
            return await this.PutAction<UserUpdateDto, User>(id, data);
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<UserUpdateDto> data)
        {
            // TODO update logged in user only, get user -> User.GetUsername()
            return await this.PatchAction<User, UserUpdateDto>(id, data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await this.DeleteAction<User>(id);
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoForUserDto>> UploadPhoto(IFormFile file)
        {
            if (Int32.TryParse(this.User.GetUserId(), out int userId))
            {
                var user = await this.userRepository.FindById(userId);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }
                
                var result = await this.photoHandlerService.AddPhotoAsync(file);

                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }

                var photo = this.photoFactory.CreateNew<Photo>();

                photo.Url = result.SecureUri.AbsoluteUri;
                photo.PublicId = result.PublicId;

                if (user.Photos.Count == 0)
                {
                    photo.IsMain = true;
                }
                
                user.Photos.Add(photo);

                if (await this.baseRepositoryInterface.SaveAll())
                {
                    return Created(photo.Url, this.mapper.Map<PhotoForUserDto>(photo));
                }

                return BadRequest("Failed to upload photo.");
            }

            return BadRequest("Invalid user id.");
        }

        [HttpPut("set-main-photo/{id}")]
        public async Task<IActionResult> SetMainPhoto(int id)
        {
            if (Int32.TryParse(this.User.GetUserId(), out int userId))
            {
                var user = await this.userRepository.FindById(userId);

                if (user == null)
                {
                    return BadRequest($"User not found with id {userId}");
                }

                var currentMain = user.Photos.FirstOrDefault(photo => photo.IsMain);

                if (currentMain != null)
                {
                    currentMain.IsMain = false;
                }
                
                var newMain = user.Photos.FirstOrDefault(photo => photo.Id == id);
                
                if (newMain == null)
                {
                    return BadRequest($"Photo not found with id {id}");
                }

                newMain.IsMain = true;

                if (await this.baseRepositoryInterface.SaveAll())
                {
                    return NoContent();
                }
                
                return BadRequest("Failed to set main photo.");
            }

            return BadRequest("Logged in user not found.");
        }
    }
}