using System.ComponentModel.DataAnnotations;

namespace Lab3.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Department name must be between 3 and 30 characters.")]
        [Display(Name = "Department Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Manager name is required.")]
        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; } = string.Empty;
    }
}