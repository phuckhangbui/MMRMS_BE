using Common;
using DTOs.Component;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Product
{
    public class CreateProductDto
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

        public IEnumerable<CreateProductAttributeDto>? ProductAttributes { get; set; }

        public IEnumerable<AddExistedComponentToProduct>? ExistedComponentList { get; set; }

        public IEnumerable<CreateComponentEmbeddedDto>? NewComponentList { get; set; }
    }

    public class CreateProductAttributeDto
    {
        [Required(ErrorMessage = MessageConstant.ProductAttribute.NameRequired)]
        public string AttributeName { get; set; }

        [Required(ErrorMessage = MessageConstant.ProductAttribute.SpecsRequired)]
        public string Specifications { get; set; }

        public string? Unit { get; set; }
    }

    public class AddExistedComponentToProduct
    {
        [Required(ErrorMessage = MessageConstant.Component.ComponentIdRequired)]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Component.QuantityPositiveNumber)]
        public int Quantity { get; set; }
    }

}
