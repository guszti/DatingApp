using System;

namespace DatingApp.API.Model
{
    public class UserLike : IUserLike
    {
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public int SourceUserId { get; set; }
        
        public User SourceUser { get; set; }
        
        public int LikedUserId { get; set; }
        
        public User LikedUser { get; set; }
    }
}