using System.Diagnostics;
using Lab3.Models;
using Lab3.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    public class HomeController : Controller
    {
        // Part 2.2 (Welcome message) + Part 3.1 (Visit counter)
        public IActionResult Index()
        {
            string userName = Request.Cookies["UserName"] ?? string.Empty;
            ViewBag.WelcomeMessage = string.IsNullOrEmpty(userName) ? "Welcome, Guest" : $"Welcome, {userName}";

            int visitCount = HttpContext.Session.GetInt32("VisitCount") ?? 0;
            visitCount++;
            HttpContext.Session.SetInt32("VisitCount", visitCount);
            ViewBag.VisitCount = visitCount;

            return View();
        }

        // Part 2.1 - Remember User Name
        [HttpGet]
        public IActionResult SetUserName()
        {
            var currentName = Request.Cookies["UserName"];
            return View(new UserNameViewModel { UserName = currentName ?? string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetUserName(UserNameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Response.Cookies.Append("UserName", viewModel.UserName, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            return RedirectToAction(nameof(Index));
        }

        // Part 2.3 - Preferred Theme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetTheme(string theme)
        {
            if (theme != "Light" && theme != "Dark")
            {
                theme = "Light";
            }

            Response.Cookies.Append("Theme", theme, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            var referer = Request.Headers["Referer"].ToString();
            return Redirect(string.IsNullOrEmpty(referer) ? Url.Action(nameof(Index))! : referer);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Part 9 - Custom error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}