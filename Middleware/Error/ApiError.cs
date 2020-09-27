using System.Net;

namespace DatingApp.API.Middleware.Error
{
    public class ApiError
    {
        private int statusCode;

        private string message;

        private string details;
        
        public ApiError(HttpStatusCode statusCode, string message, string details = null)
        {
            this.statusCode = (int) statusCode;
            this.message = message;
            this.details = details;
        }
    }
}