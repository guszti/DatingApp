using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
using DatingApp.API.Helpers;
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
        public async Task<ActionResult<Grid<UserWithPhotosDto>>> Index([FromQuery] UserParamsDto gridParamsDto)
        {
            var result = await this.userRepository.FindAll(gridParamsDto);

            Response.AddPaginationHeader(result.Page, result.Limit, result.Total, result.TotalPages);

            return Ok(result);
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
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UserUpdateDto> data)
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
            int userId = this.User.GetUserId();
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

        [HttpPut("set-main-photo/{id}")]
        public async Task<IActionResult> SetMainPhoto(int id)
        {
            int userId = this.User.GetUserId();
            var user = await this.userRepository.FindById(userId);

            if (user == null)
            {
                return NotFound($"User not found with id {userId}");
            }

            var currentMain = user.Photos.FirstOrDefault(photo => photo.IsMain);

            if (currentMain != null)
            {
                currentMain.IsMain = false;
            }

            var newMain = user.Photos.FirstOrDefault(photo => photo.Id == id);

            if (newMain == null)
            {
                return NotFound($"Photo not found with id {id}");
            }

            newMain.IsMain = true;

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Failed to set main photo.");
        }

        [HttpDelete("remove-photo/{id}")]
        public async Task<IActionResult> RemovePhoto(int id)
        {
            int userId = this.User.GetUserId();
            var user = await this.userRepository.FindById(userId);

            if (user == null)
            {
                return BadRequest($"User not found with id {userId}");
            }

            var photo = user.Photos.FirstOrDefault(item => item.Id == id);

            if (null == photo)
            {
                return NotFound("Photo not found.");
            }

            if (photo.IsMain)
            {
                return BadRequest("Photo is set to main.");
            }

            if (null != photo.PublicId)
            {
                var result = await this.photoHandlerService.RemovePhotoAsync(photo.PublicId);

                if (null != result.Error)
                {
                    return BadRequest("Failed to remove photo from Cloudinary.");
                }
            }

            user.Photos.Remove(photo);

            if (await this.userRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to remove user photo.");
        }
    }
}