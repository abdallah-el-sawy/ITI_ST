using Microsoft.EntityFrameworkCore;
using Lab3.Data;
using Lab3.Models;

namespace Lab3.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Employee>> GetAllWithDepartmentAsync(string? searchName)
        {
            var query = _context.Employees.Include(e => e.Department).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(e => e.Name.ToLower().Contains(searchName.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> IsDuplicateAsync(string name, int departmentId, int? excludeId)
        {
            return await _context.Employees.AnyAsync(e =>
                e.DepartmentId == departmentId &&
                e.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || e.Id != excludeId.Value));
        }

        public async Task<int> CountAsync() => await _context.Employees.CountAsync();

        public async Task<decimal> GetAverageSalaryAsync() => await _context.Employees.AverageAsync(e => e.Salary);

        public async Task<decimal> GetMaxSalaryAsync() => await _context.Employees.MaxAsync(e => e.Salary);

        public async Task<decimal> GetMinSalaryAsync() => await _context.Employees.MinAsync(e => e.Salary);
    }
}