namespace DTOs.Machine
{
    public class DisplayMachineDetailDto
    {
        public int MachineId { get; set; }

        public string? MachineName { get; set; }

        public string? CategoryName { get; set; }

        public double? RentPrice { get; set; }

        public double? MachinePrice { get; set; }

        public int? Quantity { get; set; }

        public string? Model { get; set; }

        public string? Origin { get; set; }

        public int? CategoryId { get; set; }

        public string? Description { get; set; }

        public DateTime? DateCreate { get; set; }

        public string? Status { get; set; }


        public IEnumerable<MachineAttributeDto>? MachineAttributeList { get; set; }

        public IEnumerable<MachineImageDto>? MachineImageList { get; set; }

        public IEnumerable<MachineComponentDto>? MachineComponentList { get; set; }

        public IEnumerable<MachineTermDto>? MachineTermList { get; set; }

        public List<double> RentPrices { get; set; }
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
