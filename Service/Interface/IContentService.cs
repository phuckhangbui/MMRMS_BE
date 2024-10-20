using DTOs.Content;
using Microsoft.AspNetCore.Http;

namespace Service.Interface
{
    public interface IContentService
    {
        Task<IEnumerable<ContentDto>> GetContents();
        Task<ContentDto> GetContentDetailById(int contentId);
        Task CreateContent(int accountCreateId, ContentCreateRequestDto contentRequestDto);
        Task UpdateContent(int accountUpdateId, int contentId, ContentUpdateRequestDto contentUpdateRequestDto);
        Task DeleteContent(int contentId);
        Task ChangeContentImage(int contentId, IFormFile imageUrl);
        Task ChangeContentStatus(int contentId, string status);
    }
}
