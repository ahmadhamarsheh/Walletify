using Microsoft.AspNetCore.Mvc;

namespace Walletify.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            double target = 10000;
            double balance = 1000;

            double progress = ((balance / target) * 100);

            ViewBag.progress = progress;
            ViewBag.target = target;
            ViewBag.balance = balance;

            return View();
        }

    }
}
