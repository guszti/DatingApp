using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Model;

namespace DatingApp.API.Repository
{
    public interface IUserLikeRepository : IBaseRepository
    {
        public Task<UserLike> getUserLikeBySourceAndTarget(int sourceId, int targetId);

        public Task<User> getUserWithLikes(int userId);

        public Task<Grid<UserLikeDto>> getUserLikes(UserLikeParamsDto userLikeParamsDto);
    }
}