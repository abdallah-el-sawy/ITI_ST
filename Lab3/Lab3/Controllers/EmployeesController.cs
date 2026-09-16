using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab3.Data;
using Lab3.Models;
using Lab3.ViewModels;

namespace Lab3.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
            return View(employees);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new EmployeeViewModel
            {
                Departments = await LoadDepartmentsAsync()
            };
            return View(viewModel);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel viewModel)
        {
            if (await IsDuplicateEmployeeAsync(viewModel.Name, viewModel.DepartmentId, null))
            {
                ModelState.AddModelError(nameof(viewModel.Name),
                    "An employee with this name already exists in the selected department.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.Departments = await LoadDepartmentsAsync();
                return View(viewModel);
            }

            var employee = new Employee
            {
                Name = viewModel.Name,
                Age = viewModel.Age,
                Salary = viewModel.Salary,
                JobTitle = viewModel.JobTitle,
                DepartmentId = viewModel.DepartmentId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                JobTitle = employee.JobTitle,
                DepartmentId = employee.DepartmentId,
                Departments = await LoadDepartmentsAsync()
            };

            return View(viewModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (await IsDuplicateEmployeeAsync(viewModel.Name, viewModel.DepartmentId, id))
            {
                ModelState.AddModelError(nameof(viewModel.Name),
                    "An employee with this name already exists in the selected department.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.Departments = await LoadDepartmentsAsync();
                return View(viewModel);
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            employee.Name = viewModel.Name;
            employee.Age = viewModel.Age;
            employee.Salary = viewModel.Salary;
            employee.JobTitle = viewModel.JobTitle;
            employee.DepartmentId = viewModel.DepartmentId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Employees/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IsDuplicateEmployeeAsync(string name, int departmentId, int? excludeId)
        {
            return await _context.Employees.AnyAsync(e =>
                e.DepartmentId == departmentId &&
                e.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || e.Id != excludeId.Value));
        }

        private async Task<List<SelectListItem>> LoadDepartmentsAsync()
        {
            return await _context.Departments
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToListAsync();
        }
    }
}