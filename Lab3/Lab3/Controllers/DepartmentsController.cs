using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab3.Data;
using Lab3.Models;
using Lab3.ViewModels;

namespace Lab3.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .ToListAsync();
            return View(departments);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View(new DepartmentViewModel());
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel viewModel)
        {
            if (await IsDuplicateDepartmentAsync(viewModel.Name, null))
            {
                ModelState.AddModelError(nameof(viewModel.Name), "A department with this name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var department = new Department
            {
                Name = viewModel.Name,
                ManagerName = viewModel.ManagerName
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            var viewModel = new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                ManagerName = department.ManagerName
            };

            return View(viewModel);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (await IsDuplicateDepartmentAsync(viewModel.Name, id))
            {
                ModelState.AddModelError(nameof(viewModel.Name), "A department with this name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            department.Name = viewModel.Name;
            department.ManagerName = viewModel.ManagerName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Employees/5  (the "Show Employees" button)
        public async Task<IActionResult> Employees(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return NotFound();

            return View(department);
        }

        private async Task<bool> IsDuplicateDepartmentAsync(string name, int? excludeId)
        {
            return await _context.Departments.AnyAsync(d =>
                d.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || d.Id != excludeId.Value));
        }
    }
}