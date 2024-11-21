using Microsoft.AspNetCore.Mvc;
using SampleProjectWeb.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using SampleProjectWeb.Models.ViewModels;

namespace SampleProjectWeb.Controllers
{
    public class HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userToCreate = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(userToCreate, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
