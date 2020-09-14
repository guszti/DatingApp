using System.Net;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
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
        private IAuthRepository authRepository;

        private IUserFactory userFactory;

        private IAuthService authService;

        public AuthController(IAuthRepository authRepository, IUserFactory userFactory, IAuthService authService)
        {
            this.authRepository = authRepository;
            this.userFactory = userFactory;
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            string username = userForRegisterDto.username.ToLower();
            string plainPassword = userForRegisterDto.password;

            if (await this.authRepository.DoesUserExist(username)) return BadRequest("Username already in use.");

            User user = this.userFactory.CreateNamed(username);

            IUser registeredUser = await this.authRepository.Register(user, plainPassword);

            return StatusCode((int) HttpStatusCode.Created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto requestUser)
        {
            IUser user = await this.authRepository.Login(requestUser.Username.ToLower(), requestUser.Password);

            if (user == null) return Unauthorized();

            string jwtToken = this.authService.CreateJwtToken(user.Username, user.Id);

            return Ok(new
            {
                token = jwtToken
            });
        }
    }
}