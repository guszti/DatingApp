using System.Text.Json;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Extensions
{
    public static class HttpResponseExtension
    {
        public static void AddPaginationHeader(this HttpResponse response, int page, int limit, int total, int totalPages)
        {
            var pagination = new Pagination(page, limit, total, totalPages);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}