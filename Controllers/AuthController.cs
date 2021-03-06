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

        private IUnitOfWork unitOfWork;

        public AuthController(
            IMapper mapper,
            IAuthService authServiceInterface,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUnitOfWork unitOfWork
        )
        {
            this.mapper = mapper;
            this.authServiceInterface = authServiceInterface;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request data.");
            }

            User user = this.mapper.Map<User>(userForRegisterDto);

            user.UserName = userForRegisterDto.username.ToLower();

            if (await this.unitOfWork.UserRepository.DoesUserExist(user.UserName))
            {
                return BadRequest("Username already in use.");
            }

            var userResult = await this.userManager.CreateAsync(user, user.PlainPassword);

            if (!userResult.Succeeded)
            {
                return BadRequest("Failed to create user.");
            }
            
            var roleResult = await this.userManager.AddToRoleAsync(user, IRole.Member);

            if (!roleResult.Succeeded)
            {
                return BadRequest("Failed to add user to role.");
            }
            
            return Created("", new
            {
                Id = user.Id,
                Username = user.UserName,
                Token = await this.authServiceInterface.CreateJwtToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            });
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
                Token = await this.authServiceInterface.CreateJwtToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url
            };

            return Created("", new
            {
                user = loggedInUserDto
            });
        }
    }
}