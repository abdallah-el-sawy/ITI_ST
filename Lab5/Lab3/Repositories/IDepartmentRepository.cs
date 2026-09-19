using Lab3.Models;

namespace Lab3.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<List<Department>> GetAllWithEmployeesAsync();
        Task<Department?> GetWithEmployeesAsync(int id);
        Task<bool> IsDuplicateAsync(string name, int? excludeId);
        Task<int> CountAsync();
    }
}