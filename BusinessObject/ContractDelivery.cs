namespace BusinessObject
{
    public partial class ContractDelivery
    {
        public int ContractDeliveryId { get; set; }

        public string? ContractId { get; set; }

        public int? DeliveryTaskId { get; set; }

        public string? PictureUrl { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        public virtual Contract? Contract { get; set; }

        public virtual DeliveryTask? DeliveryTask { get; set; }
    }
}
