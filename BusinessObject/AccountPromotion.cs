using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class AccountPromotion
{
    public int PromotionAccountId { get; set; }

    public int? PromotionId { get; set; }

    public int? AccountId { get; set; }

    public DateTime? DateReceive { get; set; }

    public int? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Promotion? Promotion { get; set; }
}
