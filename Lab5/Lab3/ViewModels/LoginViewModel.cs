using System.ComponentModel.DataAnnotations;
using Lab3.Validation;

namespace Lab3.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "User name must be between 2 and 30 characters.")]
        [NoNumbers]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}