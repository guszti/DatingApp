using System;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Extensions
{
    public static class AuthServicesExtension
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>(opt => { opt.Password.RequireNonAlphanumeric = false; })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<User>>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!String.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(IAuthService.PolicyRequireAdmin, policy => policy.RequireRole(IRole.Admin));
                opt.AddPolicy(IAuthService.PolicyRequireModerator, policy => policy.RequireRole(IRole.Moderator));
            });

            return services;
        }
    }
}