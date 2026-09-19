using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lab3.Services;
using Lab3.ViewModels;

namespace Lab3.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index(string? searchName)
        {
            if (searchName == null)
            {
                searchName = Request.Cookies["EmployeeSearch"];
            }
            else
            {
                Response.Cookies.Append("EmployeeSearch", searchName, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }

            ViewBag.SearchName = searchName;

            var employees = await _employeeService.GetEmployeesAsync(searchName);
            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new EmployeeViewModel
            {
                Departments = await _employeeService.GetDepartmentSelectListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var (success, error) = await _employeeService.CreateEmployeeAsync(viewModel);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(viewModel.Name), error!);
            }

            viewModel.Departments = await _employeeService.GetDepartmentSelectListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = await _employeeService.GetEmployeeForEditAsync(id.Value);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var (success, error) = await _employeeService.UpdateEmployeeAsync(id, viewModel);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(viewModel.Name), error!);
            }

            viewModel.Departments = await _employeeService.GetDepartmentSelectListAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}