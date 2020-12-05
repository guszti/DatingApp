using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
using DatingApp.API.Helpers;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UserLikeController : BaseController
    {
        private IUserLikeRepository userLikeRepository;

        private IUserLikeFactory userLikeFactory;

        private IUserRepository userRepository;

        public UserLikeController(
            IBaseRepository baseRepositoryInterface,
            IMapper mapper,
            IUserLikeRepository userLikeRepository,
            IUserLikeFactory userLikeFactory,
            IUserRepository userRepository
        )
            : base(baseRepositoryInterface, mapper)
        {
            this.userLikeRepository = userLikeRepository;
            this.userLikeFactory = userLikeFactory;
            this.userRepository = userRepository;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Like(int id)
        {
            int userId = this.User.GetUserId();

            if (id == userId)
            {
                return BadRequest("You cannot like yourself.");
            }

            var user = await this.baseRepositoryInterface.FindById<User>(id);

            if (null == user)
            {
                return BadRequest("Target user not found.");
            }

            var userLike = await this.userLikeRepository.getUserLikeBySourceAndTarget(userId, id);

            if (null != userLike)
            {
                return BadRequest("You have already liked this user.");
            }

            var newUserLike = this.userLikeFactory.CreateForLike(userId, id);
            var sourceUser = await this.userRepository.FindById(userId);

            sourceUser.LikedUsers.Add(newUserLike);

            if (await this.baseRepositoryInterface.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to like user.");
        }

        [HttpGet]
        public async Task<ActionResult<Grid<UserLikeDto>>> GetLikes([FromQuery] UserLikeParamsDto userLikeParamsDto)
        {
            int userId = this.User.GetUserId();

            userLikeParamsDto.UserId = userId;

            var likes = await this.userLikeRepository.getUserLikes(userLikeParamsDto);

            return Ok(likes);
        }
    }
}