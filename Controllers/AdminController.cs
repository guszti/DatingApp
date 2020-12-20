using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Model;
using DatingApp.API.Repository;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    public class AdminController : BaseController
    {
        private UserManager<User> userManager;

        private RoleManager<Role> roleManager;
        
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
            : base(unitOfWork, mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet("users-with-roles")]
        [Authorize(Policy = IAuthService.PolicyRequireAdmin)]
        public async Task<ActionResult<IEnumerable>> GetUsersWithRoles()
        {
            var result = await this.userManager.Users
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.Roles.Select(ur => ur.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost("edit-roles/{id}")]
        public async Task<IActionResult> EditRoles(int id, [FromQuery]string roles)
        {
            var rolesArray = roles.Split(',');
            var user = await this.userManager.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (null == user)
            {
                return BadRequest("User not found.");
            }
            
            var currentRoles = user.Roles
                .Select(ur => ur.Role.Name)
                .ToList();

            var rolesToRemove = currentRoles.Where(cr => !rolesArray.Contains(cr));
            
            var roleRemovalResult = await this.userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!roleRemovalResult.Succeeded)
            {
                return BadRequest("Failed to remove roles!");
            }

            var rolesToAdd = rolesArray.Where(r => !currentRoles.Contains(r));
            
            var roleAdditionResult = await this.userManager.AddToRolesAsync(user, new List<string>(rolesToAdd));

            if (!roleAdditionResult.Succeeded)
            {
                return BadRequest("Failed to add roles to user.");
            }

            return Ok();
        }

        [HttpGet("photos-to-moderate")]
        [Authorize(Policy = IAuthService.PolicyRequireModerator)]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Moderator content");
        }
    }
}