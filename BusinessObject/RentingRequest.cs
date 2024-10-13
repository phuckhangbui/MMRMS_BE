﻿namespace BusinessObject;

public partial class RentingRequest
{
    public string RentingRequestId { get; set; } = null!;

    public int? AccountOrderId { get; set; }

    public int? AddressId { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateStart { get; set; }

    public double? TotalRentPrice { get; set; }

    public double? TotalDepositPrice { get; set; }

    public double? TotalServicePrice { get; set; }

    public double? ShippingPrice { get; set; }

    public double? DiscountShip { get; set; }

    public double? DiscountPrice { get; set; }

    public int? NumberOfMonth { get; set; }

    public double? TotalAmount { get; set; }

    public bool? IsOnetimePayment { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public virtual Account? AccountOrder { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<RentingRequestProductDetail> RentingRequestProductDetails { get; set; } = new List<RentingRequestProductDetail>();

    public virtual ICollection<ServiceRentingRequest> ServiceRentingRequests { get; set; } = new List<ServiceRentingRequest>();
}
