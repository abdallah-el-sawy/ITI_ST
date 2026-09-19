using Lab3.Models;
using Lab3.Repositories;
using Lab3.ViewModels;

namespace Lab3.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _unitOfWork.Departments.GetAllWithEmployeesAsync();
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateDepartmentAsync(DepartmentViewModel viewModel)
        {
            if (await _unitOfWork.Departments.IsDuplicateAsync(viewModel.Name, null))
            {
                return (false, "A department with this name already exists.");
            }

            var department = new Department
            {
                Name = viewModel.Name,
                ManagerName = viewModel.ManagerName
            };

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return (true, null);
        }

        public async Task<DepartmentViewModel?> GetDepartmentForEditAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null) return null;

            return new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                ManagerName = department.ManagerName
            };
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateDepartmentAsync(int id, DepartmentViewModel viewModel)
        {
            if (await _unitOfWork.Departments.IsDuplicateAsync(viewModel.Name, id))
            {
                return (false, "A department with this name already exists.");
            }

            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null) return (false, "Department not found.");

            department.Name = viewModel.Name;
            department.ManagerName = viewModel.ManagerName;

            _unitOfWork.Departments.Update(department);
            await _unitOfWork.SaveChangesAsync();
            return (true, null);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department != null)
            {
                _unitOfWork.Departments.Remove(department);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Department?> GetDepartmentWithEmployeesAsync(int id)
        {
            return await _unitOfWork.Departments.GetWithEmployeesAsync(id);
        }
    }
}