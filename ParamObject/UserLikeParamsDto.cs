namespace DatingApp.API
{
    public class UserLikeParamsDto : GridParamsDto
    {
        public int UserId { get; set; }
        
        public string Predicate { get; set; }
    }
}