using Common;
using Common.Enum;
using DTOs.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class ContentServiceImpl : IContentService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IContentRepository _contentRepository;

        public ContentServiceImpl(ICloudinaryService cloudinaryService, IContentRepository contentRepository)
        {
            _cloudinaryService = cloudinaryService;
            _contentRepository = contentRepository;
        }

        public async Task ChangeContentImage(int contentId, IFormFile imageUrl)
        {
            var content = await CheckContentExist(contentId);

            string imageUrlStr = await _cloudinaryService.UploadImageToCloudinary(imageUrl);

            await _contentRepository.ChangeContentImage(contentId, imageUrlStr);
        }

        public async Task CreateContent(int accountCreateId, ContentCreateRequestDto contentRequestDto)
        {
            string imageUrlStr = await _cloudinaryService.UploadImageToCloudinary(contentRequestDto.ImageUrl);

            await _contentRepository.CreateContent(accountCreateId, contentRequestDto, imageUrlStr);
        }

        public async Task DeleteContent(int contentId)
        {
            var content = await CheckContentExist(contentId);

            await _contentRepository.DeleteContent(contentId);
        }

        public async Task<ContentDto> GetContentDetailById(int contentId)
        {
            var content = await CheckContentExist(contentId);

            return content;
        }

        public async Task<IEnumerable<ContentDto>> GetContents()
        {
            var contents = await _contentRepository.GetContents();

            if (contents.IsNullOrEmpty())
            {
                throw new ServiceException(MessageConstant.Content.ContentListEmpty);
            }

            return contents;
        }

        public async Task UpdateContent(int accountUpdateId, int contentId, ContentUpdateRequestDto contentUpdateRequestDto)
        {
            var content = await CheckContentExist(contentId);

            await _contentRepository.UpdateContent(accountUpdateId, contentId, contentUpdateRequestDto);
        }


        public async Task ChangeContentStatus(int contentId, string status)
        {
            await CheckContentExist(contentId);

            if (!Enum.IsDefined(typeof(ContentStatusEnum), status))
            {
                throw new ServiceException(MessageConstant.InvalidStatusValue);
            }

            await _contentRepository.ChangeContentStatus(contentId, status);
        }

        private async Task<ContentDto> CheckContentExist(int contentId)
        {
            var content = await _contentRepository.GetContentDetailById(contentId);

            if (content == null)
            {
                throw new ServiceException(MessageConstant.Content.ContentNotFound);
            }

            return content;
        }
    }
}
