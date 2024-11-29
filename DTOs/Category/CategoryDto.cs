using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? DateCreate { get; set; }
    }

    public class CategoryRequestDto
    {
        [Required(ErrorMessage = MessageConstant.Category.CategoryNameRequired)]
        public string CategoryName { get; set; }
    }
}
