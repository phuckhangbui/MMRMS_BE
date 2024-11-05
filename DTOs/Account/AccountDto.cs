using Common;
using DTOs.MembershipRank;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Account
{
    public class AccountDto
    {
        public int AccountId { get; set; }

        public string? AvatarImg { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? DateBirth { get; set; }

        public int? Gender { get; set; }

        public string? Address { get; set; }

        public string? Username { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public string? OtpNumber { get; set; }

        public string? FirebaseMessageToken { get; set; }

        public string? TokenRefresh { get; set; }

        public DateTime? TokenDateExpire { get; set; }

        public int? MembershipRankId { get; set; }

        public double? MoneySpent { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateExpire { get; set; }

        public int? RoleId { get; set; }

        public String? Status { get; set; }

        public bool? IsDelete { get; set; }
    }

    public class StaffAccountDto
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
    }

    public class AccountBaseDto
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateCreate { get; set; }
        public string Status { get; set; }
        public int RoleId { get; set; }
        public int Gender { get; set; }
        public string DateBirth { get; set; }
    }

    public class EmployeeAccountDto : AccountBaseDto
    {
        public string Username { get; set; }
        public string DateExpire { get; set; }
        public string DateBirth { get; set; }
        public string AvatarImg { get; set; }
    }

    public class CustomerAccountDto : AccountBaseDto
    {
        public string? AvatarImg { get; set; }
        public string? Company { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public string? TaxNumber { get; set; }
    }

    public class CustomerAccountDetailDto : CustomerAccountDto
    {
        public MembershipRankDto MembershipRank { get; set; }
    }

    public class NewBaseAccountDto
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
    }

    public class NewCustomerAccountDto : NewBaseAccountDto
    {
        [Required(ErrorMessage = MessageConstant.Account.CompanyRequired)]
        public string Company { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PositionRequired)]
        public string? Position { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.TaxNumberRequired)]
        public string? TaxNumber { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PasswordRequired)]
        public string Password { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.AddressRequired)]
        public string Address { get; set; }
    }

    public class NewEmployeeAccountDto : NewBaseAccountDto
    {
        [Required(ErrorMessage = MessageConstant.Account.UsernameRequired)]
        public string Username { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.DateExpireRequired)]
        public DateTime DateExpire { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.RoleIdRequired)]
        public int RoleId { get; set; }
    }
}
