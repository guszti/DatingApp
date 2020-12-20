using System;
using System.Threading.Tasks;
using DatingApp.API.Model;
using DatingApp.API.Extensions;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class UpdateUserLastActive : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var userId = resultContext.HttpContext.User.GetUserId();
            var unitOfWork = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();

            var user = await unitOfWork.BaseRepository.FindById<User>(userId);

            user.LastActive = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();
        }
    }
}