namespace DTOs.Machine
{
    public class MachineImageDto
    {
        public int MachineImageId { get; set; }
        public int? MachineId { get; set; }
        public string? MachineImageUrl { get; set; }
        public bool? IsThumbnail { get; set; }
    }
}
