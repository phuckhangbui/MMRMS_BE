namespace DTOs.SerialNumberProduct
{
    public class CreateSerialProductNumberDto
    {
        public string SerialNumber { get; set; } = null!;

        public int? ProductId { get; set; }
    }
}
