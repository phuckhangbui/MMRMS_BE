using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Product
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = MessageConstant.Product.ProductNameRequired)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = MessageConstant.Product.DescriptionRequired)]
        public string? Description { get; set; }

        [Required(ErrorMessage = MessageConstant.Product.ProductPriceRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Product.ProductPricePositiveNumber)]
        public double ProductPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.Product.RentPriceRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Product.RentPricePositiveNumber)]
        public double RentPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.Product.ModelRequired)]
        public string Model { get; set; }

        [Required(ErrorMessage = MessageConstant.Product.OrginRequired)]
        public string Origin { get; set; }

        [Required(ErrorMessage = MessageConstant.Product.CategoryRequired)]
        public int CategoryId { get; set; }
    }
}
