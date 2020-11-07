using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Services
{
    public interface IPhotoHandlerService
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        public Task<DeletionResult> RemovePhotoAsync(string publicId);
    }
}