namespace BusinessObject;

public partial class ContractSerialNumberProduct
{
    public int ContractSerialNumberProductId { get; set; }

    public int? ContractId { get; set; }

    public string? SerialNumber { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual SerialNumberProduct? SerialNumberProduct { get; set; }
}
