using Lab3.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    public class AccountController : Controller
    {
        // Part 3.3 - Login simulation
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            HttpContext.Session.SetString("LoggedInUser", viewModel.UserName);
            return RedirectToAction("Index", "Home");
        }

        // Part 3.4 - Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}