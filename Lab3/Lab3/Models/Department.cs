using System.ComponentModel.DataAnnotations;

namespace Lab3.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ManagerName { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}