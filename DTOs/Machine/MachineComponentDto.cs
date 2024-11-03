namespace DTOs.Machine
{
    public class MachineComponentDto
    {
        public int MachineComponentId { get; set; }

        public int? MachineId { get; set; }

        public string? ComponentName { get; set; }

        public int? ComponentId { get; set; }

        public int? Quantity { get; set; }
    }


}
