namespace DatingApp.API.Dtos
{
    public class PhotoForUserDto
    {
        public int Id { get; set; }
        
        public string Url { get; set; }
        
        public bool IsMain { get; set; }
        
        public string Description { get; set; }
        
        public string PublicId { get; set; }
    }
}