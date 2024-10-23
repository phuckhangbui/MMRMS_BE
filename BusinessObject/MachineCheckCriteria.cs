namespace BusinessObject
{
    public partial class MachineCheckCriteria
    {
        public int MachineCheckCriteriaId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<MachineCheckRequestCriteria> MachineCheckRequestCriterias { get; set; } = new List<MachineCheckRequestCriteria>();
    }
}
