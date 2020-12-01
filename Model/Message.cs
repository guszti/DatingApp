using System;

namespace DatingApp.API.Model
{
    public class Message : IMessage
    {
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; }
        
        public string Content { get; set; }
        
        public User Source { get; set; }
        
        public int SourceId { get; set; }
        
        public User Target { get; set; }
        
        public int TargetId { get; set; }

        public DateTime? SeenAt { get; set; }

        public bool SourceDeleted { get; set; }

        public bool TargetDeleted { get; set; }
    }
} 