using DatingApp.API.Model;

namespace DatingApp.API.Factory
{
    public interface IUserLikeFactory : IBaseFactory
    {
        public UserLike CreateForLike(int sourceId, int targetId);
    }
}