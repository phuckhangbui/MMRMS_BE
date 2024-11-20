using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.Content;
using DTOs.Contract;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class ContentRepository : IContentRepository
    {
        private readonly IMapper _mapper;

        public ContentRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task ChangeContentImage(int contentId, string imageUrl)
        {
            var content = await ContentDao.Instance.GetContentById(contentId);
            if (content != null)
            {
                content.ImageUrl = imageUrl;

                await ContentDao.Instance.UpdateAsync(content);
            }
        }

        public async Task ChangeContentStatus(int contentId, string status)
        {
            var content = await ContentDao.Instance.GetContentById(contentId);
            if (content != null)
            {
                content.Status = status;
                await ContentDao.Instance.UpdateAsync(content);
            }
        }

        public async Task CreateContent(int accountCreateId, ContentCreateRequestDto contentRequestDto, string imageUrl)
        {
            var content = _mapper.Map<Content>(contentRequestDto);

            content.AccountId = accountCreateId;
            content.DateCreate = DateTime.Now;
            content.ImageUrl = imageUrl;
            content.Status = ContentStatusEnum.Active.ToString();

            await ContentDao.Instance.CreateAsync(content);
        }

        public async Task DeleteContent(int contentId)
        {
            var content = await ContentDao.Instance.GetContentById(contentId);
            if (content != null)
            {
                await ContentDao.Instance.RemoveAsync(content);
            }
        }

        public async Task<ContentDto?> GetContentDetailById(int contentId)
        {
            var content = await ContentDao.Instance.GetContentById(contentId);
            if (content != null)
            {
                return _mapper.Map<ContentDto>(content);
            }

            return null;
        }

        public async Task<IEnumerable<ContentDto>> GetContents(string? status)
        {
            var contents = await ContentDao.Instance.GetContents();

            if (!contents.IsNullOrEmpty())
            {
                var contentDtos = _mapper.Map<IEnumerable<ContentDto>>(contents);

                if (!string.IsNullOrEmpty(status))
                {
                    contentDtos = contentDtos.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                }

                return contentDtos;
            }

            return [];
        }

        public async Task UpdateContent(int accountUpdateId, int contentId, ContentUpdateRequestDto contentUpdateRequestDto)
        {
            var content = await ContentDao.Instance.GetContentById(contentId);
            if (content != null)
            {
                content.AccountId = accountUpdateId;
                content.Title = contentUpdateRequestDto.Title;
                content.ContentBody = contentUpdateRequestDto.ContentBody;
                content.Summary = contentUpdateRequestDto.Summary;

                await ContentDao.Instance.UpdateAsync(content);
            }
        }
    }
}
