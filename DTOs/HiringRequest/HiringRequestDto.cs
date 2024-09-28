namespace DTOs.RentingRequest
{
    public class RentingRequestDto
    {
        public string RentingRequestId { get; set; } = null!;

        public int? AccountOrderId { get; set; }

        public string? AccountOrderName { get; set; }

        public int? AddressId { get; set; }

        public string? ContractId { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateStart { get; set; }

        public int? NumberOfMonth { get; set; }

        public bool? IsOnetimePayment { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }
    }
}
