using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Service.Cloundinary;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        private readonly string uploadFolder;
        public CloudinaryService(IOptions<CloudinarySetting> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            cloudinary = new Cloudinary(acc);
            uploadFolder = config.Value.UploadFolder;
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = uploadFolder,
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<string> UploadImageToCloudinary(IFormFile imageUrl)
        {
            var result = await AddPhotoAsync(imageUrl);
            if (result.Error != null)
            {
                //TODO
                throw new ServiceException(MessageConstant.ImageUploadError);
            }

            return result.SecureUrl.AbsoluteUri;
        }

        public async Task<string> UploadImageToCloudinary(string base64String)
        {
            try
            {
                // Check if Base64 string is null or empty
                if (string.IsNullOrEmpty(base64String))
                {
                    throw new ServiceException("Base64 image data is required.");
                }

                // Decode the Base64 string to a byte array
                byte[] imageBytes;
                try
                {
                    imageBytes = Convert.FromBase64String(base64String);
                }
                catch (FormatException)
                {
                    throw new ServiceException("Invalid Base64 string.");
                }

                // Convert the byte array to a MemoryStream (required for Cloudinary upload)
                using var memoryStream = new MemoryStream(imageBytes);

                // Generate a unique file name for the image
                var fileName = $"upload_{Guid.NewGuid()}.jpg";

                // Prepare a file for upload to Cloudinary
                var uploadParams = new ImageUploadParams
                {
                    File = new CloudinaryDotNet.FileDescription(fileName, memoryStream),
                    Folder = uploadFolder,
                };

                // Upload the image to Cloudinary
                var result = await cloudinary.UploadAsync(uploadParams);
                if (result.Error != null)
                {
                    throw new ServiceException("Error uploading image to Cloudinary: " + result.Error.Message);
                }

                // Return the uploaded image URL
                string imageUrl = result.SecureUrl.AbsoluteUri;

                return imageUrl;
            }
            catch (Exception ex)
            {
                throw new ServiceException(MessageConstant.ImageUploadError);
            }
        }

        public async Task<string[]> UploadImageToCloudinary(string[] base64Strings)
        {
            try
            {
                // Validate input array
                if (base64Strings == null || base64Strings.Length == 0)
                {
                    throw new ServiceException("Base64 image data array is required.");
                }

                var imageUrls = new List<string>();

                // Loop through each Base64 string and call the single upload method
                foreach (var base64String in base64Strings)
                {
                    if (string.IsNullOrEmpty(base64String))
                    {
                        throw new ServiceException("One or more Base64 image data are invalid.");
                    }

                    try
                    {
                        // Use the existing single upload method
                        var imageUrl = await UploadImageToCloudinary(base64String);
                        imageUrls.Add(imageUrl);
                    }
                    catch (ServiceException ex)
                    {
                        throw new ServiceException($"Error uploading one of the images: {ex.Message}");
                    }
                }

                // Return the array of uploaded image URLs
                return imageUrls.ToArray();
            }
            catch (Exception ex)
            {
                throw new ServiceException(MessageConstant.ImageUploadError, ex);
            }
        }

    }
}
