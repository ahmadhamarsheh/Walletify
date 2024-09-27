using Microsoft.AspNetCore.Mvc;

namespace Walletify.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

    }
}
