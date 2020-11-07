using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Services
{
    public class PhotoHandlerService : IPhotoHandlerService
    {
        private readonly Cloudinary cloudinary;
        
        public PhotoHandlerService(IOptions<CloudinarySettings> options)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret
                );
            
            this.cloudinary = new Cloudinary(account);
        }
        
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                return await this.cloudinary.UploadAsync(uploadParams);
            }
            
            return new ImageUploadResult();
        }

        public async Task<DeletionResult> RemovePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await this.cloudinary.DestroyAsync(deleteParams);
        }
    }
}