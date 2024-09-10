using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public string? InvoiceCode { get; set; }

    public int? ContractId { get; set; }

    public int? Method { get; set; }

    public double? Price { get; set; }

    public DateTime? DateCreate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Contract? Contract { get; set; }
}
