namespace BusinessObject
{
    public class RentingService
    {
        public int RentingServiceId { get; set; }

        public string? RentingServiceName { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }

        public bool? IsOptional { get; set; }

        public virtual ICollection<ServiceRentingRequest> ServiceRentingRequests { get; set; } = new List<ServiceRentingRequest>();
    }
}
