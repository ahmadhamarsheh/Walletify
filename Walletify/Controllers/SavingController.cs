using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Walletify.Controllers
{
    [Authorize]
    public class SavingController : Controller
    {
        public IActionResult SetValue()
        {
            return View();
        }
        public IActionResult GetValue()
        {
            return View();
        }

        public IActionResult ChangeValue()
        {
            return View();
        }
    }
}
