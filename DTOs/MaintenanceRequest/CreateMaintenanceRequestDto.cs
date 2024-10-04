using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MaintenanceRequest
{
    public class CreateMaintenanceRequestDto
    {
        [Required(ErrorMessage = MessageConstant.MaintanningTicket.ProductSerialNumberRequired)]
        public string ContractId { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.ProductSerialNumberRequired)]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = MessageConstant.MaintanningTicket.NoteRequired)]
        public string Note { get; set; }
    }
}
