using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Account
{
    public class AccountUpdateDto
    {
    }

    public interface IAccountUpdateDto
    {
        string Name { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
    }

    public class CustomerAccountUpdateDto : IAccountUpdateDto
    {
        [Required(ErrorMessage = MessageConstant.Account.NameRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.EmailRequired)]
        [EmailAddress(ErrorMessage = MessageConstant.Account.InvalidEmail)]
        public string Email { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PhoneRequired)]
        public string Phone { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.GenderRequired)]
        public int Gender { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.DateBirthRequired)]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.CompanyRequired)]
        public string Company { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PositionRequired)]
        public string? Position { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.TaxNumberRequired)]
        public string? TaxNumber { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.AddressRequired)]
        public string Address { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.AvatarUrlRequired)]
        public string AvatarImg { get; set; }
    }

    public class EmployeeAccountUpdateDto
    {
        //[Required(ErrorMessage = MessageConstant.Account.NameRequired)]
        //public string Name { get; set; }

        //[Required(ErrorMessage = MessageConstant.Account.EmailRequired)]
        //[EmailAddress(ErrorMessage = MessageConstant.Account.InvalidEmail)]
        //public string Email { get; set; }

        //[Required(ErrorMessage = MessageConstant.Account.PhoneRequired)]
        //public string Phone { get; set; }

        //[Required(ErrorMessage = MessageConstant.Account.GenderRequired)]
        //public int Gender { get; set; }

        //[Required(ErrorMessage = MessageConstant.Account.DateBirthRequired)]
        //public DateTime DateBirth { get; set; }

        //[Required(ErrorMessage = MessageConstant.Account.UsernameRequired)]
        //public string Username { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.RoleIdRequired)]
        public int RoleId { get; set; }

        //[Required(ErrorMessage = MessageConstant.Account.DateExpireRequired)]
        //public DateTime DateExpire { get; set; }
    }

    public class EmployeeProfileUpdateDto : IAccountUpdateDto
    {
        [Required(ErrorMessage = MessageConstant.Account.NameRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.EmailRequired)]
        [EmailAddress(ErrorMessage = MessageConstant.Account.InvalidEmail)]
        public string Email { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PhoneRequired)]
        public string Phone { get; set; }
    }
}
