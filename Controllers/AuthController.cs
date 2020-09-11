using System.Net;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Factory;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository authRepository;

        private IUserFactory userFactory;
        
        public AuthController(IAuthRepository authRepository, IUserFactory userFactory)
        {
            this.authRepository = authRepository;
            this.userFactory = userFactory;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            string username = userForRegisterDto.username.ToLower();
            string plainPassword = userForRegisterDto.password;

            if (await this.authRepository.DoesUserExist(username)) return BadRequest("Username already in use.");

            User user = this.userFactory.CreateNamed(username);

            User registeredUser = await this.authRepository.Register(user, plainPassword);

            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}