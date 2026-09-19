using Microsoft.AspNetCore.Mvc.Rendering;
using Lab3.Models;
using Lab3.ViewModels;

namespace Lab3.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync(string? searchName);
        Task<List<SelectListItem>> GetDepartmentSelectListAsync();
        Task<(bool Success, string? ErrorMessage)> CreateEmployeeAsync(EmployeeViewModel viewModel);
        Task<EmployeeViewModel?> GetEmployeeForEditAsync(int id);
        Task<(bool Success, string? ErrorMessage)> UpdateEmployeeAsync(int id, EmployeeViewModel viewModel);
        Task DeleteEmployeeAsync(int id);
    }
}