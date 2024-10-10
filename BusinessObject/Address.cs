namespace BusinessObject;

public partial class Address
{
    public int AddressId { get; set; }

    public int? AccountId { get; set; }

    public string? AddressBody { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public bool? IsDelete { get; set; }

    public string? Coordinates { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<RentingRequest> RentingRequests { get; set; } = new List<RentingRequest>();
}
