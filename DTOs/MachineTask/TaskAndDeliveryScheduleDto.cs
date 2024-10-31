namespace DTOs.MachineTask
{
    public class TaskAndDeliveryScheduleDto
    {
        public int? StaffId { get; set; }

        public DateOnly? DateStart { get; set; }

        public string? Type { get; set; }

        public int? MachineTaskId { get; set; }

        public int? DeliveryTaskId { get; set; }

        public string? Status { get; set; }
    }
}
