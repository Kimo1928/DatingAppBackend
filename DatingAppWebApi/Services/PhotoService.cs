using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAppWebApi.Helpers;
using DatingAppWebApi.Interfaces;
using Microsoft.Extensions.Options;

namespace DatingAppWebApi.Services
{
    public class PhotoService : IPhotoService
    {

        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);

        }
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
           var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
        {
            var uploadResult= new ImageUploadResult();
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "DatingAppPhotos"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
                return uploadResult;
        }
    }
}
