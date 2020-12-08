using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Extensions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class UserLikeRepository : BaseRepository, IUserLikeRepository
    {
        public UserLikeRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
            
        }

        public async Task<UserLike> getUserLikeBySourceAndTarget(int sourceId, int targetId)
        {
            return await this.context.UserLike
                .FirstOrDefaultAsync(ul => ul.SourceUserId == sourceId && ul.LikedUserId == targetId);
        }

        public async Task<User> getUserWithLikes(int userId)
        {
            return await this.context.User
                .Include(u => u.LikedUsers)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Grid<UserLikeDto>> getUserLikes(UserLikeParamsDto userLikeParamsDto)
        {
            var users = this.context.User.OrderBy(u => u.UserName).AsQueryable();
            var userLikes = this.context.UserLike.AsQueryable();

            if (userLikeParamsDto.Predicate == "liked")
            {
                userLikes = userLikes.Where(ul => ul.SourceUserId == userLikeParamsDto.UserId);
                users = userLikes.Select(ul => ul.LikedUser);
            }

            if (userLikeParamsDto.Predicate == "likedBy")
            {
                userLikes = userLikes.Where(ul => ul.LikedUserId == userLikeParamsDto.UserId);
                users = userLikes.Select(ul => ul.SourceUser);
            }

            var result = users.Select(user => new UserLikeDto
            {
                UserId = user.Id,
                Age = user.DateOfBirth.CalculateAge(),
                City = user.City,
                Username = user.UserName,
                KnownAs = user.KnownAs,
                MainPhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url
            });

            return await Grid<UserLikeDto>.CreateGridAsync(result, userLikeParamsDto.Page, userLikeParamsDto.Limit);
        }
    }
}