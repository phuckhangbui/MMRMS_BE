namespace DTOs.MachineCheckRequest
{
    public class MachineCheckRequestCriteriaDto
    {
        public int MachineCheckRequestCriteriaId { get; set; }
        public string? MachineCheckRequestId { get; set; }
        public int? MachineCheckCriteriaId { get; set; }
        public string? CriteriaName { get; set; }
        public string? CustomerNote { get; set; }
    }
}
