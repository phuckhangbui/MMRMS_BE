using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Account
{
    public class AccountUpdateDto
    {
    }

    public class EmployeeAccountUpdateDto
    {
        [Required(ErrorMessage = MessageConstant.Account.NameRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.EmailRequired)]
        [EmailAddress(ErrorMessage = MessageConstant.Account.InvalidEmail)]
        public string Email { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PhoneRequired)]
        public string Phone { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.UsernameRequired)]
        public string Username { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.RoleIdRequired)]
        public int RoleId { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.GenderRequired)]
        public int Gender { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.DateBirthRequired)]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.DateExpireRequired)]
        public DateTime DateExpire { get; set; }
    }
}
