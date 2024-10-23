using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ComponentReplacementTicket
{
    public class CreateComponentReplacementTicketDto
    {
        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.ComponentIdRequired)]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.MachineSerialNumberRequired)]
        public string MachineSerialNumber { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.ComponentIdRequired)]
        public string ContractId { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.PriceRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.ComponentReplacementTicket.PricePositiveNumberRequired)]
        public double ComponentPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.ComponentReplacementTicket.QuantityPositiveNumberRequired)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.AdditionFeeRequired)]
        public double AdditionalFee { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.TypeRequired)]
        public int Type { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.NoteRequired)]
        public string Note { get; set; }

    }
}
