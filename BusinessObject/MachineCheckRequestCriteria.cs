﻿namespace BusinessObject
{
    public partial class MachineCheckRequestCriteria
    {
        public int MachineCheckRequestCriteriaId { get; set; }
        public string? MachineCheckRequestId { get; set; }
        public string? CriteriaName { get; set; }
        public string? CustomerNote { get; set; }
        public virtual MachineCheckRequest? MachineCheckRequest { get; set; }
    }
}
