using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ContractPayment
{
    public int ContractPaymentId { get; set; }

    public int? ContractId { get; set; }

    public string? Title { get; set; }

    public double? Price { get; set; }

    public DateTime? CustomerPaidDate { get; set; }

    public DateTime? SystemPaidDate { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Contract? Contract { get; set; }
}
