using System.ComponentModel.DataAnnotations;

namespace DTOs.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }


        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }
        public int Quantity { get; set; }
    }

    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }
    }
}
