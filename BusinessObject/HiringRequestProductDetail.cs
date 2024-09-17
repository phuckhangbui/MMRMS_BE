namespace BusinessObject;

public partial class HiringRequestProductDetail
{
    public int HiringRequestProductDetailId { get; set; }

    public string? HiringRequestId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public virtual HiringRequest? HiringRequest { get; set; }

    public virtual Product? Product { get; set; }
}
