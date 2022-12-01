using IdentityProject1.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject1.Controllers
{
    public class AccountsController : Controller
    {
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
    }
}
