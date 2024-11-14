using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Machine
{
    public class CreateMachineDto
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

        [Required(ErrorMessage = MessageConstant.Machine.ShipPriceRequired)]
        public double ShipPricePerKm { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.WeightRequired)]
        public double Weight { get; set; }

        public IEnumerable<CreateMachineAttributeDto>? MachineAttributes { get; set; }

        public IEnumerable<AddExistedComponentToMachine>? ExistedComponentList { get; set; }

        public IEnumerable<CreateComponentEmbeddedDto>? NewComponentList { get; set; }

        [Required(ErrorMessage = MessageConstant.Machine.ImageIsRequired)]
        public IEnumerable<ImageList>? ImageUrls { get; set; }

        public IEnumerable<CreateMachineTermDto>? MachineTerms { get; set; }

    }

    public class CreateMachineAttributeDto
    {
        [Required(ErrorMessage = MessageConstant.MachineAttribute.NameRequired)]
        public string AttributeName { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineAttribute.SpecsRequired)]
        public string Specifications { get; set; }



        public string? Unit { get; set; }


    }

    public class CreateMachineTermDto
    {
        [Required(ErrorMessage = MessageConstant.MachineTerm.TitleRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTerm.ContentRequired)]
        public string Content { get; set; }
    }

    public class AddExistedComponentToMachine
    {
        [Required(ErrorMessage = MessageConstant.Component.ComponentIdRequired)]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Component.QuantityPositiveNumber)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.IsRequiredMoneyRequire)]
        public bool IsRequiredMoney { get; set; }
    }

    public class CreateComponentEmbeddedDto
    {
        [Required(ErrorMessage = MessageConstant.Component.ComponentNameRequired)]
        public string ComponentName { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.Component.QuantityPositiveNumber)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.IsRequiredMoneyRequire)]
        public bool IsRequiredMoney { get; set; }

        [Required(ErrorMessage = MessageConstant.Component.PriceRequired)]
        [Range(1, Double.MaxValue, ErrorMessage = MessageConstant.Component.PricePositiveNumber)]
        public double Price { get; set; }
    }

    public class ComponentList
    {
        public IEnumerable<AddExistedComponentToMachine>? ExistedComponentList { get; set; }
        public IEnumerable<CreateComponentEmbeddedDto>? NewComponentList { get; set; }
    }

    public class ImageList
    {
        [Required(ErrorMessage = MessageConstant.Machine.ImageIsRequired)]
        public string Url { get; set; }
    }

}
