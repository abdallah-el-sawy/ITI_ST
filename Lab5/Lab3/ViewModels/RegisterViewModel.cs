using System.ComponentModel.DataAnnotations;
using Lab3.Validation;

namespace Lab3.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "User name must be between 2 and 30 characters.")]
        [NoNumbers]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}