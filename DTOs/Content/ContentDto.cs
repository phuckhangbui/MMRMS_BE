using Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Content
{
    public class ContentDto
    {
        public int ContentId { get; set; }
        public string? Title { get; set; }

        public string? ImageUrl { get; set; }

        public string? Summary { get; set; }

        public string? ContentBody { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }

        public string? AccountCreateName { get; set; }
    }

    public class ContentCreateRequestDto
    {
        [Required(ErrorMessage = MessageConstant.Content.TitleRequired)]
        public string? Title { get; set; }

        [Required(ErrorMessage = MessageConstant.Content.SummaryRequired)]
        public string? Summary { get; set; }

        [Required(ErrorMessage = MessageConstant.Content.ContentRequired)]
        public string? ContentBody { get; set; }

        [Required(ErrorMessage = MessageConstant.Content.ImageUrlRequired)]
        public IFormFile ImageUrl { get; set; }
    }

    public class ContentUpdateRequestDto
    {
        [Required(ErrorMessage = MessageConstant.Content.TitleRequired)]
        public string? Title { get; set; }

        [Required(ErrorMessage = MessageConstant.Content.SummaryRequired)]
        public string? Summary { get; set; }

        [Required(ErrorMessage = MessageConstant.Content.ContentRequired)]
        public string? ContentBody { get; set; }
    }
}
