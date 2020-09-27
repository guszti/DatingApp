using System;
using DatingApp.API.Data;
using DatingApp.API.Factory;
using DatingApp.API.Repository;
using DatingApp.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace DatingApp.API.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DataContext, DataContext>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddDbContextPool<DataContext>(options =>
                options.UseMySql(configuration.GetConnectionString("connection_string"),
                    mysqlOptions => mysqlOptions.ServerVersion(new Version(8, 0, 18), ServerType.MySql)));

            return services;
        }
    }
}