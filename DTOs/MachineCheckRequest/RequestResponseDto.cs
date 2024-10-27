namespace DTOs.MachineCheckRequest
{
    public class RequestResponseDto
    {
        public int RequestResponseId { get; set; }

        public string? MachineCheckRequestId { get; set; }

        public int? MachineTaskId { get; set; }

        public DateTime? DateResponse { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Action { get; set; }
    }
}
