using BlogApp.Core.DTOs;
using BlogApp.Core.Iservices;
using BlogApp.Core.Models;
using BlogApp.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogApp.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IAuthService userService,
             ILogger<AuthController> logger,
             SignInManager<User> signInManager
            )
        {
            _userService = userService;
            _logger = logger;
            _signInManager = signInManager;
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
                    await _userService.RegisterAsync(model);
                    return RedirectToAction("Auth", "Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during registration.");
                }
            }

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.LoginAsync(model.username, model.password);

            if (result.Success)
            {
                await _signInManager.SignInWithClaimsAsync(
                    result.User,
                    isPersistent: false,
                    result.Claims);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }
    }
}
