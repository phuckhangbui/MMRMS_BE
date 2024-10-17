using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.EmployeeTask
{
    public class CreateEmployeeTaskDto
    {
        [Required(ErrorMessage = MessageConstant.EmployeeTask.RequestIdRequired)]
        public string RequestId { get; set; }

        [Required(ErrorMessage = MessageConstant.EmployeeTask.StaffIdRequired)]
        public int StaffId { get; set; }

        [Required(ErrorMessage = MessageConstant.EmployeeTask.TitleRequired)]
        public string TaskTitle { get; set; }

        [Required(ErrorMessage = MessageConstant.EmployeeTask.TaskContentRequired)]
        public string TaskContent { get; set; }

        [Required(ErrorMessage = MessageConstant.EmployeeTask.DateStartRequired)]
        public DateTime DateStart { get; set; }

        public string? Note { get; set; }
    }
}
