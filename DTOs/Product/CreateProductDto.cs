using DTOs.Component;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Product
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Origin is required")]
        public string Origin { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }

        public IEnumerable<CreateProductAttributeDto>? ProductAttributes { get; set; }

        public IEnumerable<int>? ComponentList { get; set; }

        public IEnumerable<CreateComponentEmbeddedDto>? ComponentListEmbedded { get; set; }
    }

    public class CreateProductAttributeDto
    {
        [Required(ErrorMessage = "AttributeName is required")]
        public string? AttributeName { get; set; }

        [Required(ErrorMessage = "Specifications is required")]
        public string? Specifications { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        public string? Unit { get; set; }
    }

}
