using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab3.Validation;

namespace Lab3.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [NoNumbers]
        public string Name { get; set; } = string.Empty;

        [Range(20, 60, ErrorMessage = "Age must be between 20 and 60.")]
        [MinimumAgeForHighSalary(nameof(Salary), 20000, 22)]
        public int Age { get; set; }

        [Range(typeof(decimal), "5000", "50000", ErrorMessage = "Salary must be between 5000 and 50000.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a department.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a department.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public List<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    }
}