using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
    public class PasswordDto
    {
        [Required(ErrorMessage = "Current password is required"), MaxLength(100)]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = "New Password is required"), MaxLength(100)]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Please confirm the password")]
        [Compare("NewPassword", ErrorMessage = "Confirm Password and New Password do not match")]
        public string ConfirmPassword { get; set; } = "";
    }
}
