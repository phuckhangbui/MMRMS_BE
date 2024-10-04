namespace DTOs.MaintenanceTicket
{
    public class CreateMaintenanceTicketDto
    {
        public int ComponentId { get; set; }

        public string ProductSerialNumber { get; set; }

        public string ContractId { get; set; }

        public double ComponentPrice { get; set; }

        public int Quantity { get; set; }

        public double AdditionalFee { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }

    }
}
