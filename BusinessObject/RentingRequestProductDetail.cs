namespace BusinessObject;

public partial class RentingRequestProductDetail
{
    public int RentingRequestProductDetailId { get; set; }

    public string? RentingRequestId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public double? RentPrice { get; set; }

    public double? DepositPrice { get; set; }

    public virtual RentingRequest? RentingRequest { get; set; }

    public virtual Product? Product { get; set; }
}
