using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.RentingService
{
    public class RentingServiceDto
    {
        public int RentingServiceId { get; set; }
        public string RentingServiceName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsOptional { get; set; }
    }

    public class RentingServiceRequestDto
    {
        [Required(ErrorMessage = MessageConstant.RentingService.RentingServiceNameRequired)]
        public string RentingServiceName { get; set; }

        [Required(ErrorMessage = MessageConstant.RentingService.DescriptionRequired)]
        public string Description { get; set; }

        public double Price { get; set; }

        public bool IsOptional { get; set; }
    }
}
