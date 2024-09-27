using Microsoft.AspNetCore.Mvc;

namespace Walletify.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult GetUserProfile(int id)
        {
            return View();
        }
        public IActionResult UpdateUserProfile()
        {
            return View();
        }

    }
}
