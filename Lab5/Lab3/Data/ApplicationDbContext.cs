using Microsoft.EntityFrameworkCore;
using Lab3.Models;

namespace Lab3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "IT", ManagerName = "John Smith" },
                new Department { Id = 2, Name = "HR", ManagerName = "Sara Ahmed" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Ahmed Ali", Age = 25, Salary = 15000, JobTitle = "Developer", DepartmentId = 1 },
                new Employee { Id = 2, Name = "Mona Adel", Age = 30, Salary = 22000, JobTitle = "HR Specialist", DepartmentId = 2 }
            );
        }
    }
}