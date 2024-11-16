namespace BusinessObject;

public partial class RentingRequest
{
    public string RentingRequestId { get; set; } = null!;

    public int? AccountOrderId { get; set; }

    public DateTime? DateCreate { get; set; }

    public double? TotalRentPrice { get; set; }

    public double? TotalDepositPrice { get; set; }

    public double? TotalServicePrice { get; set; }

    public double? ShippingPrice { get; set; }

    public double? DiscountPrice { get; set; }

    public double? TotalAmount { get; set; }

    public bool? IsOnetimePayment { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public string? AccountNumber { get; set; }

    public string? BeneficiaryBank { get; set; }

    public string? BeneficiaryName { get; set; }

    public virtual Account? AccountOrder { get; set; }

    public virtual RentingRequestAddress? RentingRequestAddress { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<ServiceRentingRequest> ServiceRentingRequests { get; set; } = new List<ServiceRentingRequest>();
}
