using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Services
{
    public interface IAuthService
    {
        public const string PolicyRequireAdmin = "RequireAdminRole";
        public const string PolicyRequireModerator = "RequireModeratorRole";
        
        public Task<string> CreateJwtToken(User user);
    }
}