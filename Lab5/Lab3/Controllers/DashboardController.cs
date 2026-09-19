using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lab3.Repositories;
using Lab3.ViewModels;

namespace Lab3.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalEmployees = await _unitOfWork.Employees.CountAsync(),
                TotalDepartments = await _unitOfWork.Departments.CountAsync()
            };

            if (viewModel.TotalEmployees > 0)
            {
                viewModel.AverageSalary = await _unitOfWork.Employees.GetAverageSalaryAsync();
                viewModel.HighestSalary = await _unitOfWork.Employees.GetMaxSalaryAsync();
                viewModel.LowestSalary = await _unitOfWork.Employees.GetMinSalaryAsync();
            }

            return View(viewModel);
        }
    }
}