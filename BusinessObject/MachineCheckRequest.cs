﻿namespace BusinessObject;

public partial class MachineCheckRequest
{
    public string MachineCheckRequestId { get; set; }

    public string? ContractId { get; set; }

    public int? MachineTaskId { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreate { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual MachineTask? MachineTask { get; set; }

    public virtual ICollection<MachineCheckRequestCriteria> MachineCheckRequestCriterias { get; set; } = new List<MachineCheckRequestCriteria>();

}
