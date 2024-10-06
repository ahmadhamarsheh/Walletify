using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Walletify.Controllers
{
    [Authorize]
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
