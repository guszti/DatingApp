using System;
using DatingApp.API.Enum;

namespace DatingApp.API.Dtos
{
    // TODO password hashing
    public class UserCreateDto
    {
        public string Username { get; set; }

        public string PlainPassword { get; set; }

        public Gender Gender { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string KnownAs { get; set; }
        
        public DateTime LastActive { get; set; }
        
        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }
        
        public string Interests { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
    }
}