using System;
using System.Collections.Generic;
using DatingApp.API.Enum;

namespace DatingApp.API.Dtos
{
    public class UserWithPhotosDto
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public Gender Gender { get; set; }
        
        public int Age { get; set; }
        
        public string KnownAs { get; set; }
        
        public DateTime LastActive { get; set; }
        
        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }
        
        public string Interests { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public string MainPhotoUrl { get; set; }
        
        public ICollection<PhotoForUserDto> Photos { get; set; }
    }
}