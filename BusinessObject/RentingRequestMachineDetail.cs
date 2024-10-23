namespace BusinessObject;

public partial class RentingRequestMachineDetail
{
    public int RentingRequestMachineDetailId { get; set; }

    public string? RentingRequestId { get; set; }

    public int? MachineId { get; set; }

    public int? Quantity { get; set; }

    public virtual RentingRequest? RentingRequest { get; set; }

    public virtual Machine? Machine { get; set; }
}
