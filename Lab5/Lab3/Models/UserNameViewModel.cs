using System.ComponentModel.DataAnnotations;
using Lab3.Validation;

namespace Lab3.ViewModels
{
    public class UserNameViewModel
    {
        [Required(ErrorMessage = "Please enter a user name.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "User name must be between 2 and 30 characters.")]
        [NoNumbers]
        public string UserName { get; set; } = string.Empty;
    }
}