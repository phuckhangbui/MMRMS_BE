namespace DTOs.EmployeeTask
{
    public class EmployeeTaskDto
    {
        public int EmployeeTaskId { get; set; }

        public string? TaskTitle { get; set; }

        public string? ContractId { get; set; }

        public string? Content { get; set; }

        public int? StaffId { get; set; }

        public string? StaffName { get; set; }

        public int? ManagerId { get; set; }

        public string? ManagerName { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }


    }
}
