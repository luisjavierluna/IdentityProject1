using IdentityProject1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject1.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            SignInViewModel signInViewModel = new SignInViewModel();
            return View(signInViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
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
                    //return RedirectToAction("Index", "Home");
                    return LocalRedirect(returnUrl);
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
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    loginViewModel.Email, 
                    loginViewModel.Password, 
                    loginViewModel.RememberMe, 
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    //return RedirectToAction("Index", "Home");
                    return LocalRedirect(returnUrl);
                }
                else if (result.IsLockedOut)
                {
                    return View("Blocked");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Acces");
                    return View(loginViewModel);
                }
            }

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel fpViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(fpViewModel.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var urlReturn = Url.Action(
                    "ResetPassword", 
                    "Accounts", 
                    new { userId = user.Id, code = code }, 
                    protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(fpViewModel.Email, "Recover password - Identity Project", 
                    "Please, click here to recover your password: <a href=\"" + urlReturn + "\">enlace</a>");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(fpViewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}
