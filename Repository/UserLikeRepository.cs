using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Extensions;
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

        public async Task<IEnumerable<UserLikeDto>> getUserLikes(int userId, string predicate)
        {
            var users = this.context.User.OrderBy(u => u.Username).AsQueryable();
            var userLikes = this.context.UserLike.AsQueryable();

            if (predicate == "liked")
            {
                userLikes = userLikes.Where(ul => ul.SourceUserId == userId);
                users = userLikes.Select(ul => ul.LikedUser);
            }

            if (predicate == "likedBy")
            {
                userLikes = userLikes.Where(ul => ul.LikedUserId == userId);
                users = userLikes.Select(ul => ul.SourceUser);
            }

            return await users.Select(user => new UserLikeDto
            {
                UserId = user.Id,
                Age = user.DateOfBirth.CalculateAge(),
                City = user.City,
                Username = user.Username,
                KnownAs = user.KnownAs,
                MainPhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url
            }).ToListAsync();
        }
    }
}