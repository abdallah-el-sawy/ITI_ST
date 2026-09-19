using Lab3.Models;
using Lab3.ViewModels;

namespace Lab3.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetDepartmentsAsync();
        Task<(bool Success, string? ErrorMessage)> CreateDepartmentAsync(DepartmentViewModel viewModel);
        Task<DepartmentViewModel?> GetDepartmentForEditAsync(int id);
        Task<(bool Success, string? ErrorMessage)> UpdateDepartmentAsync(int id, DepartmentViewModel viewModel);
        Task DeleteDepartmentAsync(int id);
        Task<Department?> GetDepartmentWithEmployeesAsync(int id);
    }
}