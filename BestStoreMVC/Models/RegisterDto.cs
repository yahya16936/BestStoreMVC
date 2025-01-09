using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The First Name field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "the Last Name field is required"), MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Format of the Phone Number is Invalid"), MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; } = "";

        [Required, MaxLength(200)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Please confirm the password")]
        [Compare("Password", ErrorMessage = "Confirm Password and Password do not match")]
        public string ConfirmPassword { get; set; } = ""; 

    }
}
