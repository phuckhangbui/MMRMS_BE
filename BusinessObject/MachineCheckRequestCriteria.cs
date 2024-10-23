namespace BusinessObject
{
    public partial class MachineCheckRequestCriteria
    {
        public int MachineCheckRequestCriteriaId { get; set; }
        public string? MachineCheckRequestId { get; set; }
        public int? MachineCheckCriteriaId { get; set; }
        public string? CustomerNote { get; set; }
        public virtual MachineCheckRequest? MachineCheckRequest { get; set; }
        public virtual MachineCheckCriteria? MachineCheckCriteria { get; set; }
    }
}
