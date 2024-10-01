namespace DTOs.MaintenanceRequest
{
    public class CreateMaintenanceRequestDto
    {
        public string ContractId { get; set; }

        public string SerialNumber { get; set; }

        public string Note { get; set; }
    }
}
