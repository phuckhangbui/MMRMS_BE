using DTOs.ComponentReplacementTicket;
using DTOs.Contract;
using DTOs.MachineCheckRequest;

namespace DTOs.MachineTask
{
    public class MachineTaskDisplayDetail
    {
        public int MachineTaskId { get; set; }

        public string? TaskTitle { get; set; }

        public string? ContractId { get; set; }

        public string? SerialNumber { get; set; }

        public string? MachineCheckRequestId { get; set; }

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

        public string? ConfirmationPictureUrl { get; set; }

        public ContractAddressDto? Address { get; set; }

        public MachineCheckRequestDetailDto? MachineCheckRequest { get; set; }

        public IEnumerable<ComponentReplacementTicketDto>? ComponentReplacementTicketCreateFromTaskList { get; set; }

        public IEnumerable<MachineTaskLogDto>? TaskLogs { get; set; }

    }
}
