using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IUserLikeRepository : IBaseRepository
    {
        public Task<UserLike> getUserLikeBySourceAndTarget(int sourceId, int targetId);

        public Task<User> getUserWithLikes(int userId);

        public Task<IEnumerable<UserLikeDto>> getUserLikes(int userId, string predicate);
    }
}