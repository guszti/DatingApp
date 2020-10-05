using System;
using System.Collections.Generic;
using DatingApp.API.Enum;

namespace DatingApp.API.Model
{
    public interface IUser : IEntity
    {
        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public Gender Gender { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string KnownAs { get; set; }
        
        public DateTime LastActive { get; set; }
        
        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }
        
        public string Interests { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public ICollection<Photo> Photos { get; set; }
    }
}