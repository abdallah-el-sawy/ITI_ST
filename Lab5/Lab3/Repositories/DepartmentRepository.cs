using Microsoft.EntityFrameworkCore;
using Lab3.Data;
using Lab3.Models;

namespace Lab3.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Department>> GetAllWithEmployeesAsync()
        {
            return await _context.Departments.Include(d => d.Employees).ToListAsync();
        }

        public async Task<Department?> GetWithEmployeesAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> IsDuplicateAsync(string name, int? excludeId)
        {
            return await _context.Departments.AnyAsync(d =>
                d.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || d.Id != excludeId.Value));
        }

        public async Task<int> CountAsync() => await _context.Departments.CountAsync();
    }
}