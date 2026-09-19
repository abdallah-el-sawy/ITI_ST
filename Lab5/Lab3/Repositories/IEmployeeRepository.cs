using Lab3.Models;

namespace Lab3.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<List<Employee>> GetAllWithDepartmentAsync(string? searchName);
        Task<bool> IsDuplicateAsync(string name, int departmentId, int? excludeId);
        Task<int> CountAsync();
        Task<decimal> GetAverageSalaryAsync();
        Task<decimal> GetMaxSalaryAsync();
        Task<decimal> GetMinSalaryAsync();
    }
}