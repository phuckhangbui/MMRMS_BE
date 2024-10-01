namespace DTOs.MaintainingTicket
{
    public class CreateMaintaningTicketDto
    {
        public int ComponentId { get; set; }

        public string ProductSerialNumber { get; set; }

        public double ComponentPrice { get; set; }

        public double AdditionalFee { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }

    }
}
