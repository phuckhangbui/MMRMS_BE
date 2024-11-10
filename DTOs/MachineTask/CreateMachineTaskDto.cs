using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.MachineTask
{
    public abstract class CreateMachineTaskDtoBase
    {
        [Required(ErrorMessage = MessageConstant.MachineTask.StaffIdRequired)]
        public int StaffId { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTask.TitleRequired)]
        public string TaskTitle { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTask.TaskContentRequired)]
        public string TaskContent { get; set; }

        [Required(ErrorMessage = MessageConstant.MachineTask.DateStartRequired)]
        public string DateStart { get; set; }

        public string? Note { get; set; }
    }

    public class CreateMachineTaskCheckRequestDto : CreateMachineTaskDtoBase
    {
        [Required(ErrorMessage = MessageConstant.MachineTask.RequestIdRequired)]
        public string RequestId { get; set; }
    }

    public class CreateMachineTaskContractTerminationDto : CreateMachineTaskDtoBase
    {
        [Required(ErrorMessage = MessageConstant.MachineTask.ContractIdRequired)]
        public string ContractId { get; set; }
    }

    public class CreateMachineTaskDeliveryFailDto : CreateMachineTaskDtoBase
    {
        [Required(ErrorMessage = MessageConstant.MachineTask.ContractIdRequired)]
        public int ContractDeliveryId { get; set; }
    }
}
