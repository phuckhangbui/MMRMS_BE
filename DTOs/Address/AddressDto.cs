using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Address
{
    public class AddressDto
    {
        public int AddressId { get; set; }
        public int AccountId { get; set; }
        public string AddressBody { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Coordinates { get; set; }
        public bool IsDelete { get; set; }
    }

    public class AddressRequestDto
    {
        [Required(ErrorMessage = MessageConstant.Address.AddressBodyRequired)]
        public string AddressBody { get; set; }

        [Required(ErrorMessage = MessageConstant.Address.DistrictRequired)]
        public string District { get; set; }

        [Required(ErrorMessage = MessageConstant.Address.CityRequired)]
        public string City { get; set; }

        public string Coordinates { get; set; }
    }
}
