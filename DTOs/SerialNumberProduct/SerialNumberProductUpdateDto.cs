using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.SerialNumberProduct
{
    public class SerialNumberProductUpdateDto
    {
        [Required(ErrorMessage = MessageConstant.SerialNumberProduct.ActualRentPriceRequired)]
        public double ActualRentPrice { get; set; }
        [Required(ErrorMessage = MessageConstant.SerialNumberProduct.RentTimeCounterequired)]
        public int RentTimeCounter { get; set; }

    }
}
