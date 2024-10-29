namespace DTOs.MachineSerialNumber
{
    public class MachineSerialNumberComponentDto
    {
        public int MachineSerialNumberComponentId { get; set; }

        public string? SerialNumber { get; set; }

        public int? ComponentId { get; set; }

        public string? ComponentName { get; set; }

        public int? Quantity { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public int? ComponentInStoreQuantity { get; set; }

        public double? ComponentPrice { get; set; }

    }
}
