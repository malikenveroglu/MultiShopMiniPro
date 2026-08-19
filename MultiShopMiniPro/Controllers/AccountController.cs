using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.Models;
using MultiShopMiniPro.ViewModels;

namespace MultiShopMiniPro.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser
            {
                Name = userVM.Name,
                SurName = userVM.Surname,
                UserName = userVM.Username,
                Email = userVM.Email,
                Gender = userVM.Gender
            };

            IdentityResult result = await _userManager.CreateAsync(user, userVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(userVM);
            }

            await _signInManager.SignInAsync(user, false);

            if (returnUrl is not null) return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userVM.EmailOrUsername || u.Email == userVM.EmailOrUsername);

            if(user is null)
            {
                ModelState.AddModelError(string.Empty, "Email, UserName or Password is Incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, userVM.IsPersisted, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Try Again After 3 Minutes");
                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email, UserName or Password is Incorrect");
                    return View();
                } 
            }

            if(returnUrl is not null) return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout(string? returnUrl)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl is not null) return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}
