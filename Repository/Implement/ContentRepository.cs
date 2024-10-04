using AutoMapper;
using BusinessObject;
using DAO;
using Common.Enum;
using DTOs.Content;
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

        public async Task CreateContent(ContentCreateRequestDto contentRequestDto, string imageUrl)
        {
            var content = _mapper.Map<Content>(contentRequestDto);

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

        public async Task<IEnumerable<ContentDto>> GetContents()
        {
            var contents = await ContentDao.Instance.GetAllAsync();

            if (!contents.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<ContentDto>>(contents);
            }

            return [];
        }

        public async Task UpdateContent(int contentId, ContentUpdateRequestDto contentUpdateRequestDto)
        {
            var content = await ContentDao.Instance.GetContentById(contentId);
            if (content != null)
            {
                content.Title = contentUpdateRequestDto.Title;
                content.ContentBody = contentUpdateRequestDto.ContentBody;
                content.Summary = contentUpdateRequestDto.Summary;

                await ContentDao.Instance.UpdateAsync(content);
            }
        }
    }
}
