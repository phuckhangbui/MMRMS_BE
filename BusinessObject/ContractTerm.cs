namespace BusinessObject;

public partial class ContractTerm
{
    public int ContractTermId { get; set; }

    public string? ContractId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Contract? Contract { get; set; }
}
