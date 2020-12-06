using System;

namespace DatingApp.API.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string Content { get; set; }
        
        public string SourceUsername { get; set; }
        
        public string SourcePhotoUrl { get; set; }
        
        public string TargetUsername { get; set; }
        
        public string TargetPhotoUrl { get; set; }
        
        public DateTime? SeenAt { get; set; }
    }
}