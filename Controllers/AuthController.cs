using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMapper mapper;

        private IAuthService authServiceInterface;

        private UserManager<User> userManager;

        private SignInManager<User> signInManager;

        private IUserRepository userRepository;

        public AuthController(
            IMapper mapper,
            IAuthService authServiceInterface,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserRepository userRepository
        )
        {
            this.mapper = mapper;
            this.authServiceInterface = authServiceInterface;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request data.");
            }

            User user = this.mapper.Map<User>(userForRegisterDto);

            if (await this.userRepository.DoesUserExist(user.UserName)) return BadRequest("Username already in use.");

            var registeredUser = await this.userManager.CreateAsync(user, user.PlainPassword);

            return Created("", registeredUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedInUserDto>> Login(UserForLoginDto requestUser)
        {
            var user = await this.userManager.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.UserName == requestUser.Username);

            if (user == null) return Unauthorized("Invalid username.");

            var result = await this.signInManager.CheckPasswordSignInAsync(user, requestUser.Password, false);

            LoggedInUserDto loggedInUserDto = new LoggedInUserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Token = this.authServiceInterface.CreateJwtToken(user.UserName, user.Id),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url
            };

            return Created("", new
            {
                user = loggedInUserDto
            });
        }
    }
}