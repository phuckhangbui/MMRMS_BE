using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ComponentReplacementTicket
{
    public abstract class CreateComponentReplacementTicketBaseDto
    {

    }

    public class CreateComponentReplacementTicketDto : CreateComponentReplacementTicketBaseDto
    {
        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.MachineTaskIdRequired)]
        public int MachineTaskCreateId { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.MachineSerialNumberComponentIdRequired)]
        public int MachineSerialNumberComponentId { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.PriceRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.ComponentReplacementTicket.PricePositiveNumberRequired)]
        public double ComponentPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.ComponentReplacementTicket.QuantityPositiveNumberRequired)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.AdditionFeeRequired)]
        public double AdditionalFee { get; set; }

        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.NoteRequired)]
        public string? Note { get; set; }
    }

    public class CreateComponentReplacementTicketContractTerminationDto : CreateComponentReplacementTicketBaseDto
    {
        [Required(ErrorMessage = MessageConstant.ComponentReplacementTicket.ContractIdRequired)]
        public int ContractId { get; set; }
    }
}
