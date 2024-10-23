namespace DTOs.Machine
{
    public class MachineReviewDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public int Quantity { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<double> RentPrices { get; set; }
    }
}
