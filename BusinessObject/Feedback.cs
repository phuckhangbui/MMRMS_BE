using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? AccountFeedbackId { get; set; }

    public string? FeedbackImg { get; set; }

    public string? Content { get; set; }

    public int? ContractId { get; set; }

    public DateTime? DateCreate { get; set; }

    public int? Status { get; set; }

    public virtual Account? AccountFeedback { get; set; }

    public virtual Contract? Contract { get; set; }
}
