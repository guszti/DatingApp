using System;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Factory;
using DatingApp.API.Helpers;
using DatingApp.API.Repository;
using DatingApp.API.Services;
using DatingApp.API.SignalR;
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
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<DataContext, DataContext>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddAutoMapper(typeof(AutomapperProfiles).Assembly);
            services.AddScoped<IPhotoHandlerService, PhotoHandlerService>();
            services.AddScoped<IPhotoFactory, PhotoFactory>();
            services.AddScoped<UpdateUserLastActive>();
            services.AddScoped<IUserLikeFactory, UserLikeFactory>();
            services.AddScoped<IMessageFactory, MessageFactory>();
            services.AddScoped<IGroupFactory, GroupFactory>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddDbContextPool<DataContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("connection_string"),
                    new MySqlServerVersion(new Version(8, 0, 22)),
                    mysqlOptions => mysqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)
                )
            );

            return services;
        }
    }
}