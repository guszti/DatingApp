using System;
using System.ComponentModel.DataAnnotations;
using DatingApp.API.Enum;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 100 characters.")]
        public string username { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 255 characters.")]
        public string plainPassword { get; set; }
        
        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender gender { get; set; }
        
        [Required]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 255 characters.")]
        public string knownAs { get; set; }
        
        [Required]
        public DateTime dateOfBirth { get; set; }
        
        [Required]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 255 characters.")]
        public string city { get; set; }
        
        [Required]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 255 characters.")]
        public string country { get; set; }
    }
}