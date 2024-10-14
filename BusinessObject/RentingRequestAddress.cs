namespace BusinessObject;

public partial class RentingRequestAddress
{
    public string? RentingRequestId { get; set; }

    public string? AddressBody { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? Coordinates { get; set; }

    public virtual RentingRequest? RentingRequest { get; set; }
}
