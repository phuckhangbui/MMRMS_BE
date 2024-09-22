using Common;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Authentication
{
    public class LoginUsernameDto
    {
        [Required(ErrorMessage = MessageConstant.Account.UsernameRequired)]
        public string Username { get; set; }


        [Required(ErrorMessage = MessageConstant.Account.PasswordRequired)]
        public string Password { get; set; }
    }


    public class LoginEmailDto
    {
        [Required(ErrorMessage = MessageConstant.Account.EmailRequired)]
        [EmailAddress(ErrorMessage = MessageConstant.Account.InvalidEmail)]
        public string Email { get; set; }

        [Required(ErrorMessage = MessageConstant.Account.PasswordRequired)]
        public string Password { get; set; }
    }
}
