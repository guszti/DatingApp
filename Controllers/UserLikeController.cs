using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Extensions;
using DatingApp.API.Factory;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UserLikeController : BaseController
    {
        private IUserLikeRepository userLikeRepository;

        private IUserLikeFactory userLikeFactory;
            
        public UserLikeController(
            IBaseRepository baseRepositoryInterface, 
            IMapper mapper,
            IUserLikeRepository userLikeRepository,
            IUserLikeFactory userLikeFactory
            )
            : base(baseRepositoryInterface, mapper)
        {
            this.userLikeRepository = userLikeRepository;
            this.userLikeFactory = userLikeFactory;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> LikeUser(int id)
        {
            if (Int32.TryParse(User.GetUserId(), out int userId)) {
                if (id == userId)
                {
                    return BadRequest("You cannot like yourself.");
                }

                var user = await this.baseRepositoryInterface.FindById<User>(id);

                if (null == user)
                {
                    return BadRequest("Target user not found.");
                }

                var userLike = this.userLikeRepository.getUserLikeBySourceAndTarget(userId, id);

                if (null != userLike)
                {
                    return BadRequest("You have already liked this user.");
                }

                var newUserLike = this.userLikeFactory.CreateForLike(userId, id);
                var sourceUser = await this.baseRepositoryInterface.FindById<User>(userId);

                sourceUser.LikedUsers.Add(newUserLike);

                if (await this.baseRepositoryInterface.SaveAll())
                {
                    return Ok();
                }

                return BadRequest("Failed to like user.");
            }

            return BadRequest("User not found.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLikeDto>>> GetUserLikes([FromQuery] string predicate)
        {
            if (Int32.TryParse(User.GetUserId(), out int userId))
            {
                var likes = await this.userLikeRepository.getUserLikes(userId, predicate);

                return Ok(likes);
            }

            return BadRequest("Current user not found.");
        }
    }
}