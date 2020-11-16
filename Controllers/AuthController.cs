using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMapper mapper;
        
        private IAuthRepository authRepositoryInterface;
        
        private IAuthService authServiceInterface;

        public AuthController(
            IMapper mapper,
            IAuthRepository authRepositoryInterface, 
            IAuthService authServiceInterface
        )
        {
            this.mapper = mapper;
            this.authRepositoryInterface = authRepositoryInterface;
            this.authServiceInterface = authServiceInterface;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request data.");
            }
            
            User user = this.mapper.Map<User>(userForRegisterDto);
            
            if (await this.authRepositoryInterface.DoesUserExist(user.Username)) return BadRequest("Username already in use.");
            
            IUser registeredUser = await this.authRepositoryInterface.Register(user);

            return Created("", registeredUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedInUserDto>> Login(UserForLoginDto requestUser)
        {
            IUser user = await this.authRepositoryInterface.Login(requestUser.Username.ToLower(), requestUser.Password);

            if (user == null) return Unauthorized("Invalid username.");

            string jwtToken = this.authServiceInterface.CreateJwtToken(user.Username, user.Id);

            LoggedInUserDto loggedInUserDto = new LoggedInUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Token = jwtToken,
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url
            };
            
            return Created("", new
            {
                user = loggedInUserDto
            });
        }
    }
}