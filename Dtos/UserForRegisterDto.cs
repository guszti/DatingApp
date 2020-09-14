using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 100 characters.")]
        public string username { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 255 characters.")]
        public string password { get; set; }
    }
}