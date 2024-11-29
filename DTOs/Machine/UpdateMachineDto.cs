using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Machine
{
    public class UpdateMachineDto
    {
        [Required(ErrorMessage = MessageConstant.Machine.MachineNameRequired)]
        public string MachineName { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.DescriptionRequired)]
        public string? Description { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.MachinePriceRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Machine.MachinePricePositiveNumber)]
        public double MachinePrice { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.RentPriceRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Machine.RentPricePositiveNumber)]
        public double RentPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.ModelRequired)]
        public string Model { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.OrginRequired)]
        public string Origin { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.CategoryRequired)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.WeightRequired)]
        public double Weight { get; set; }
    }
}
