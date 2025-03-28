﻿using DTOs.Content;

namespace Repository.Interface
{
    public interface IContentRepository
    {
        Task<IEnumerable<ContentDto>> GetContents(string? status);
        Task<ContentDto?> GetContentDetailById(int contentId);
        Task CreateContent(int accountCreateId, ContentCreateRequestDto contentRequestDto, string imageUrl);
        Task UpdateContent(int accountUpdateId, int contentId, ContentUpdateRequestDto contentUpdateRequestDto);
        Task DeleteContent(int contentId);
        Task ChangeContentImage(int contentId, string imageUrl);
        Task ChangeContentStatus(int contentId, string status);
    }
}
