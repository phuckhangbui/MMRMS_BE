using System.ComponentModel.DataAnnotations;

namespace DTOs.Authentication
{
    public class EmailRegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Citizen Card is required")]
        public string CitizenCard { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}
