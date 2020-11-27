using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public class UserLikeFactory : BaseFactory, IUserLikeFactory
    {
        public UserLike CreateForLike(int sourceId, int targetId)
        {
            var userLike = this.CreateNew<UserLike>();

            userLike.SourceUserId = sourceId;
            userLike.LikedUserId = targetId;

            return userLike;
        }
    }
}