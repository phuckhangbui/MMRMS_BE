namespace BusinessObject;

public partial class HiringRequest
{
    public string HiringRequestId { get; set; } = null!;

    public int? AccountOrderId { get; set; }

    public int? AddressId { get; set; }

    public string? ContractId { get; set; }

    public DateTime? DateCreate { get; set; }

    public string? Status { get; set; }

    public virtual Account? AccountOrder { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ICollection<HiringRequestProductDetail> HiringRequestProductDetails { get; set; } = new List<HiringRequestProductDetail>();
}
