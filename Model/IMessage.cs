using System;

namespace DatingApp.API.Model
{
    public interface IMessage : IEntity
    {
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