using Lab3.Data;
using Lab3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalDepartments = await _context.Departments.CountAsync()
            };

            if (viewModel.TotalEmployees > 0)
            {
                viewModel.AverageSalary = await _context.Employees.AverageAsync(e => e.Salary);
                viewModel.HighestSalary = await _context.Employees.MaxAsync(e => e.Salary);
                viewModel.LowestSalary = await _context.Employees.MinAsync(e => e.Salary);
            }

            return View(viewModel);
        }
    }
}