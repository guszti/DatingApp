using System;
using System.Security.Claims;

namespace DatingApp.API.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}