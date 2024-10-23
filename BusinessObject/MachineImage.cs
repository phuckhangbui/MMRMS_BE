namespace BusinessObject
{
    public class MachineImage
    {
        public int MachineImageId { get; set; }
        public int? MachineId { get; set; }
        public string? MachineImageUrl { get; set; }
        public bool? IsThumbnail { get; set; }
        public virtual Machine? Machine { get; set; }
    }
}
