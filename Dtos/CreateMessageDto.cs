namespace DatingApp.API.Dtos
{
    public class CreateMessageDto
    {
        public int TargetUserId { get; set; }
        
        public string Content { get; set; }
    }
}