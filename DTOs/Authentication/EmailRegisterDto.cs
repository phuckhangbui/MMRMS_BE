using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Authentication
{
    public class EmailRegisterDto
    {
        [Required(ErrorMessage = MessageConstant.Account.NameRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.EmailRequired)]
        [EmailAddress(ErrorMessage = MessageConstant.Account.InvalidEmail)]
        public string Email { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PasswordRequired)]
        public string Password { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.AddressRequired)]
        public string Address { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PhoneRequired)]
        public string Phone { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.CitizenCardRequired)]
        public string CitizenCard { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.GenderRequired)]
        public int Gender { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.DateBirthRequired)]
        public DateTime DateBirth { get; set; }
    }
}
