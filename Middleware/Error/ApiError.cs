using System.Net;

namespace DatingApp.API.Middleware.Error
{
    public class ApiError
    {
        public int StatusCode { get; }

        public string Message { get; }

        public string Details { get; }
        
        public ApiError(HttpStatusCode statusCode, string message, string details = null)
        {
            this.StatusCode = (int) statusCode;
            this.Message = message;
            this.Details = details;
        }
    }
}