using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class UrlDto
    {
        [Required]
        public string UrlCancel { get; set; }

        [Required]
        public string UrlReturn { get; set; }

    }
}
