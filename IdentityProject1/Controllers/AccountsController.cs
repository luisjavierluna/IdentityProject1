using IdentityProject1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject1.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            SignInViewModel signInViewModel = new SignInViewModel();
            return View(signInViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = signInViewModel.Email,
                    Email = signInViewModel.Email,
                    Name = signInViewModel.Name,
                    Url = signInViewModel.Url,
                    CountryCode = signInViewModel.CountryCode,
                    Phone = signInViewModel.Phone,
                    Country = signInViewModel.Country,
                    City = signInViewModel.City,
                    Direction = signInViewModel.Direction,
                    BirthDay = signInViewModel.BirthDay,
                    State = signInViewModel.State,
                };

                var result = await _userManager.CreateAsync(user, signInViewModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                ValidateErrors(result);
            }

            return View(signInViewModel);
        }

        private void ValidateErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
