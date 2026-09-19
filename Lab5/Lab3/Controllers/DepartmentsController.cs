using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lab3.Extensions;
using Lab3.Services;
using Lab3.ViewModels;

namespace Lab3.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View(new DepartmentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var (success, error) = await _departmentService.CreateDepartmentAsync(viewModel);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(viewModel.Name), error!);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = await _departmentService.GetDepartmentForEditAsync(id.Value);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var (success, error) = await _departmentService.UpdateDepartmentAsync(id, viewModel);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(viewModel.Name), error!);
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Employees(int id)
        {
            var department = await _departmentService.GetDepartmentWithEmployeesAsync(id);
            if (department == null) return NotFound();

            var recent = HttpContext.Session.GetObject<List<string>>("RecentDepartments") ?? new List<string>();
            recent.Remove(department.Name);
            recent.Insert(0, department.Name);
            if (recent.Count > 5) recent = recent.Take(5).ToList();
            HttpContext.Session.SetObject("RecentDepartments", recent);

            ViewBag.RecentDepartments = recent;

            return View(department);
        }
    }
}