using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ComponentReplacementTicket
{
    public class CreateComponentReplacementTicketDto
    {
        [Required(ErrorMessage = MessageConstant.MaintanningTicket.ComponentIdRequired)]
        public int ComponentId { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.ProductSerialNumberRequired)]
        public string ProductSerialNumber { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.ComponentIdRequired)]
        public string ContractId { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.PriceRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.MaintanningTicket.PricePositiveNumberRequired)]
        public double ComponentPrice { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.QuantityRequired)]
        [Range(0, Double.MaxValue, ErrorMessage = MessageConstant.MaintanningTicket.QuantityPositiveNumberRequired)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.AdditionFeeRequired)]
        public double AdditionalFee { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.TypeRequired)]
        public int Type { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.NoteRequired)]
        public string Note { get; set; }

    }
}
