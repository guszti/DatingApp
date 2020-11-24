namespace DatingApp.API.Model
{
    public interface IUserLike : IEntity
    {
        public int SourceUserId { get; set; }
        
        public User SourceUser { get; set; }
        
        public int LikedUserId { get; set; }
        
        public User LikedUser { get; set; }
    }
}