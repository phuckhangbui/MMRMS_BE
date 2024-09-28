using System.ComponentModel.DataAnnotations;

namespace DTOs.SerialNumberProduct
{
    public class SerialNumberProductRentRequestDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string SerialNumber { get; set; }
    }
}
