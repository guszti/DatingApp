using System;
using System.Text.Json.Serialization;

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
        
        [JsonIgnore]
        public bool SourceDeleted { get; set; }
        
        [JsonIgnore]
        public bool TargetDeleted { get; set; }
        
        [JsonIgnore]
        public int SourceId { get; set; }
        
        [JsonIgnore]
        public int TargetId { get; set; }
    }
}