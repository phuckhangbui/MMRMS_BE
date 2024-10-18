using DTOs.Contract;
using DTOs.MaintenanceTicket;

namespace DTOs.EmployeeTask
{
    public class EmployeeTaskDisplayDetail
    {
        public int EmployeeTaskId { get; set; }

        public string? TaskTitle { get; set; }

        public string? ContractId { get; set; }

        public int? PreviousTaskId { get; set; }

        public int? MaintenanceTicketId { get; set; }

        public string? Content { get; set; }

        public int? StaffId { get; set; }

        public string? StaffName { get; set; }

        public int? ManagerId { get; set; }

        public string? ManagerName { get; set; }

        public int? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }

        public ContractAddressDto? Address { get; set; }

        public IEnumerable<MaintenanceTicketDto>? MaintenanceTicketCreateFromTaskList { get; set; }

        public IEnumerable<TaskLogDto>? TaskLogs { get; set; }

    }
}
