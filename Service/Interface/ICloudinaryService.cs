using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;

namespace Service.Interface
{
	public interface ICloudinaryService
	{
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task<DeletionResult> DeletePhotoAsync(string publicId);
	}
}
