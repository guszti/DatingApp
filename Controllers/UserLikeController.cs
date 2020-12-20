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
        private IUserLikeFactory userLikeFactory;

        public UserLikeController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserLikeFactory userLikeFactory
        )
            : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userLikeFactory = userLikeFactory;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Like(int id)
        {
            int userId = this.User.GetUserId();

            if (id == userId)
            {
                return BadRequest("You cannot like yourself.");
            }

            var user = await this.unitOfWork.BaseRepository.FindById<User>(id);

            if (null == user)
            {
                return BadRequest("Target user not found.");
            }

            var userLike = await this.unitOfWork.UserLikeRepository.getUserLikeBySourceAndTarget(userId, id);

            if (null != userLike)
            {
                return BadRequest("You have already liked this user.");
            }

            var newUserLike = this.userLikeFactory.CreateForLike(userId, id);
            var sourceUser = await this.unitOfWork.UserRepository.FindById(userId);

            sourceUser.LikedUsers.Add(newUserLike);

            if (await this.unitOfWork.SaveChangesAsync())
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

            var likes = await this.unitOfWork.UserLikeRepository.getUserLikes(userLikeParamsDto);

            return Ok(likes);
        }
    }
}