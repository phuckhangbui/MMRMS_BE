namespace DTOs.MachineTask
{
    public class TaskAndDeliveryScheduleDto
    {
        public int? StaffId { get; set; }

        public string? StaffName { get; set; }

        public DateOnly? DateStart { get; set; }

        public string? Type { get; set; }

        public int? MachineTaskId { get; set; }

        public int? DeliveryTaskId { get; set; }

        public string? Status { get; set; }
    }

    public class StaffScheduleCounterDto
    {
        public int? StaffId { get; set; }

        public string? StaffName { get; set; }

        public DateOnly? DateStart { get; set; }

        public bool? CanReceiveMoreTask { get; set; }

        public int? TaskCounter { get; set; }

        public IEnumerable<TaskAndDeliveryScheduleDto>? TaskAndDeliverySchedules { get; set; }
    }
}
