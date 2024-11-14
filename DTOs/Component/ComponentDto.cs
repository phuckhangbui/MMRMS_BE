using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Component
{
    public class ComponentDto
    {
        public int ComponentId { get; set; }

        public string? ComponentName { get; set; }

        public int? AvailableQuantity { get; set; }

        public double? Price { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }

        public int? QuantityOnHold { get; set; }
    }

    public class CreateComponentDto
    {
        [Required(ErrorMessage = MessageConstant.Component.ComponentNameRequired)]
        public string ComponentName { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Component.QuantityPositiveNumber)]
        public int AvailableQuantity { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.PriceRequired)]
        [Range(1, Double.MaxValue, ErrorMessage = MessageConstant.Component.PricePositiveNumber)]
        public double Price { get; set; }
    }




    public class UpdateComponentDto
    {
        [Required(ErrorMessage = MessageConstant.Component.ComponentIdRequired)]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Component.QuantityPositiveNumber)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.PriceRequired)]
        [Range(1, Double.MaxValue, ErrorMessage = MessageConstant.Component.PricePositiveNumber)]
        public double Price { get; set; }

    }


}
