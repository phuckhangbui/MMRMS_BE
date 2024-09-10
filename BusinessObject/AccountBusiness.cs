using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class AccountBusiness
{
    public int AccountBusinessId { get; set; }

    public int? AccountId { get; set; }

    public string? Company { get; set; }

    public string? Address { get; set; }

    public string? Position { get; set; }

    public virtual Account? Account { get; set; }
}
