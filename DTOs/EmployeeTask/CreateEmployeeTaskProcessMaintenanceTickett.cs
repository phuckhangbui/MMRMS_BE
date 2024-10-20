using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineTask
{
    public class CreateMachineTaskProcessComponentReplacementTickett
    {
        [Required(ErrorMessage = MessageConstant.MachineTask.ComponentReplacementTicketIdRequired)]
        public string ComponentReplacementTicketId { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTask.StaffIdRequired)]
        public int StaffId { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTask.TitleRequired)]
        public string TaskTitle { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTask.TaskContentRequired)]
        public string TaskContent { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTask.DateStartRequired)]
        public DateTime DateStart { get; set; }
    }
}
