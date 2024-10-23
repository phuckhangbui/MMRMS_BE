namespace BusinessObject
{
    public class MachineTerm
    {
        public int MachineTermId { get; set; }

        public int? MachineId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public virtual Machine? Machine { get; set; }
    }
}
