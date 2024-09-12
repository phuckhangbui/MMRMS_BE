using System.ComponentModel.DataAnnotations;

namespace DTOs.Account
{
    public class AccountBaseDto
    {
        public int AccountID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateCreate { get; set; }
        public string Address { get; set; }
        public string CitizenCard { get; set; }
        public string Status { get; set; }
        public int RoleID { get; set; }
        public int Gender { get; set; }
    }

    public class StaffAndManagerAccountDto : AccountBaseDto
    {
        public string Username { get; set; }
        public int Position { get; set; }
        public string DateExpire { get; set; }
        public string DateBirth { get; set; }
    }

    public class CustomerAccountDto : AccountBaseDto
    {
        public string? AvatarImg { get; set; }
        public int? BusinessType { get; set; }
        public string? Company { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public string? TaxNumber { get; set; }
    }

    public class NewBaseAccountDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } 

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } 

        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; } 

        [Required(ErrorMessage = "Citizen Card is required")]
        public string CitizenCard { get; set; } 

        [Required(ErrorMessage = "Gender is required")]
        public int Gender { get; set; }
    }

    public class NewCustomerAccountDto : NewBaseAccountDto
    {
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Company is required")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public string? Position { get; set; }

        [Required(ErrorMessage = "TaxNumber is required")]
        public string? TaxNumber { get; set; }

        [Required(ErrorMessage = "Business Type is required")]
        public int BusinessType { get; set; }
    }

    public class NewStaffAndManagerAccountDto : NewBaseAccountDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } 

        [Required(ErrorMessage = "Date of expire is required")]
        public DateTime DateExpire { get; set; }

        [Required(ErrorMessage = "DateBirth is required")]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        public int RoleID { get; set; }
    }
}
