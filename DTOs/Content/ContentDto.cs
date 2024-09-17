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
    }

    public class ContentCreateRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Summary is required")]
        public string? Summary { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string? ContentBody { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        public IFormFile ImageUrl { get; set; }
    }

    public class ContentUpdateRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Summary is required")]
        public string? Summary { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string? ContentBody { get; set; }
    }
}
