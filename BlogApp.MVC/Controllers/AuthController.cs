using BlogApp.Core.DTOs;
using BlogApp.Core.Iservices;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _userService;

        // Constructor injection of the service
        public AuthController(IAuthService userService)
        {
            _userService = userService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.RegisterAsync(model.username, model.password);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during registration.");
                }
            }

            return View(model);
        }
    }
}
