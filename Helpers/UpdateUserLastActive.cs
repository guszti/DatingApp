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
            var repo = resultContext.HttpContext.RequestServices.GetService<IBaseRepository>();

            var user = await repo.FindById<User>(userId);

            user.LastActive = DateTime.Now;

            await repo.SaveAll();
        }
    }
}