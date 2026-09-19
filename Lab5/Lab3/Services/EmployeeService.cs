using Microsoft.AspNetCore.Mvc.Rendering;
using Lab3.Models;
using Lab3.Repositories;
using Lab3.ViewModels;

namespace Lab3.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Employee>> GetEmployeesAsync(string? searchName)
        {
            return await _unitOfWork.Employees.GetAllWithDepartmentAsync(searchName);
        }

        public async Task<List<SelectListItem>> GetDepartmentSelectListAsync()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return departments
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                .ToList();
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateEmployeeAsync(EmployeeViewModel viewModel)
        {
            if (await _unitOfWork.Employees.IsDuplicateAsync(viewModel.Name, viewModel.DepartmentId, null))
            {
                return (false, "An employee with this name already exists in the selected department.");
            }

            var employee = new Employee
            {
                Name = viewModel.Name,
                Age = viewModel.Age,
                Salary = viewModel.Salary,
                JobTitle = viewModel.JobTitle,
                DepartmentId = viewModel.DepartmentId
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            return (true, null);
        }

        public async Task<EmployeeViewModel?> GetEmployeeForEditAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null) return null;

            return new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                JobTitle = employee.JobTitle,
                DepartmentId = employee.DepartmentId,
                Departments = await GetDepartmentSelectListAsync()
            };
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateEmployeeAsync(int id, EmployeeViewModel viewModel)
        {
            if (await _unitOfWork.Employees.IsDuplicateAsync(viewModel.Name, viewModel.DepartmentId, id))
            {
                return (false, "An employee with this name already exists in the selected department.");
            }

            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null) return (false, "Employee not found.");

            employee.Name = viewModel.Name;
            employee.Age = viewModel.Age;
            employee.Salary = viewModel.Salary;
            employee.JobTitle = viewModel.JobTitle;
            employee.DepartmentId = viewModel.DepartmentId;

            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.SaveChangesAsync();
            return (true, null);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee != null)
            {
                _unitOfWork.Employees.Remove(employee);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}