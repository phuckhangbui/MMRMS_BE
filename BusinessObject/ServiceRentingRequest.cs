namespace BusinessObject
{
    public class ServiceRentingRequest
    {
        public int ServiceRentingRequestId { get; set; }

        public int? RentingServiceId { get; set; }

        public string? RentingRequestId { get; set; }

        public double? ServicePrice { get; set; }

        public virtual RentingRequest? RentingRequest { get; set; }

        public virtual RentingService? RentingService { get; set; }
    }
}
