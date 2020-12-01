namespace DatingApp.API.Dtos
{
    public class CreateMessageDto
    {
        public string SourceUserId { get; set; }
        
        public string Content { get; set; }
    }
}