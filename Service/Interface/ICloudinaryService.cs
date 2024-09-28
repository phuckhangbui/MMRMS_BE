using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Service.Interface
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<string> UploadImageToCloudinary(IFormFile imageUrl);

    }
}
