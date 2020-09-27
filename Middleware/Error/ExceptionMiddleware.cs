using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Middleware.Error
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        private readonly ILogger<ExceptionMiddleware> logger;

        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.requestDelegate = requestDelegate;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.requestDelegate(context);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                ApiError apiError = this.env.IsDevelopment()
                    ? new ApiError(HttpStatusCode.InternalServerError, e.Message, e.StackTrace)
                    : new ApiError(HttpStatusCode.InternalServerError, "Internal server error.");
                
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(apiError, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}