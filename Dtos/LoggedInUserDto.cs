namespace DatingApp.API.Dtos
{
    public class LoggedInUserDto
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Token { get; set; }
        
        public string PhotoUrl { get; set; }
    }
}