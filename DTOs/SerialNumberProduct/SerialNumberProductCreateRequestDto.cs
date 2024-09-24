using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.SerialNumberProduct
{
    public class SerialNumberProductCreateRequestDto
    {
        [Required(ErrorMessage = MessageConstant.SerialNumberProduct.SerialNumberRequired)]
        public string SerialNumber { get; set; } = null!;

        [Required(ErrorMessage = MessageConstant.SerialNumberProduct.ProductIdRequired)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = MessageConstant.SerialNumberProduct.ForceWhenNoComponentInProductRequired)]
        public bool ForceWhenNoComponentInProduct { get; set; }
    }
}
