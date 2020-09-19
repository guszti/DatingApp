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
        private IAuthRepository authRepositoryInterface;

        private IUserFactory userFactoryInterface;

        private IAuthService authServiceInterface;

        public AuthController(IAuthRepository authRepositoryInterface, IUserFactory userFactoryInterface, IAuthService authServiceInterface)
        {
            this.authRepositoryInterface = authRepositoryInterface;
            this.userFactoryInterface = userFactoryInterface;
            this.authServiceInterface = authServiceInterface;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            string username = userForRegisterDto.username.ToLower();
            string plainPassword = userForRegisterDto.password;

            if (await this.authRepositoryInterface.DoesUserExist(username)) return BadRequest("Username already in use.");

            User user = this.userFactoryInterface.CreateNamed(username);

            IUser registeredUser = await this.authRepositoryInterface.Register(user, plainPassword);

            return StatusCode((int) HttpStatusCode.Created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto requestUser)
        {
            IUser user = await this.authRepositoryInterface.Login(requestUser.Username.ToLower(), requestUser.Password);

            if (user == null) return Unauthorized();

            string jwtToken = this.authServiceInterface.CreateJwtToken(user.Username, user.Id);

            return Ok(new
            {
                token = jwtToken
            });
        }
    }
}