namespace DTOs.Machine
{
    public class MachineDetailDto : MachineDto
    {
        public IEnumerable<MachineAttributeDto>? MachineAttributeList { get; set; }
        public IEnumerable<MachineImageDto>? MachineImageList { get; set; }
        public IEnumerable<MachineComponentDto>? MachineComponentList { get; set; }
        public IEnumerable<MachineTermDto>? MachineTermList { get; set; }
    }

    public class MachineAttributeDto
    {
        public int MachineAttributeId { get; set; }
        public int? MachineId { get; set; }
        public string? AttributeName { get; set; }
        public string? Specifications { get; set; }
        public string? Unit { get; set; }
    }

    public class MachineTermDto
    {
        public int MachineTermId { get; set; }
        public int? MachineId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

    }
}
