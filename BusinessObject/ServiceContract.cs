namespace BusinessObject
{
    public class ServiceContract
    {
        public int ServiceContractId { get; set; }

        public int? RentingServiceId { get; set; }

        public string? ContractId { get; set; }

        public double? ServicePrice { get; set; }

        public double? DiscountPrice { get; set; }

        public double? FinalPrice { get; set; }

        public virtual Contract? Contract { get; set; }

        public virtual RentingService? RentingService { get; set; }
    }
}
