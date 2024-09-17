using System.ComponentModel.DataAnnotations;

namespace DTOs.Component
{
    public class ComponentDto
    {
        public int ComponentId { get; set; }

        public string? ComponentName { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }
    }

    public class CreateComponentDto
    {
        [Required(ErrorMessage = "Component name is required")]
        public string ComponentName { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(minimum: 0, maximum: Double.MaxValue)]
        public double? Price { get; set; }
    }

    public class CreateComponentEmbeddedDto
    {
        [Required(ErrorMessage = "Component name is required")]
        public string ComponentName { get; set; }
    }


}
