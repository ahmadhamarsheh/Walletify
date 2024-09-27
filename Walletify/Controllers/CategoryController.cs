using Microsoft.AspNetCore.Mvc;

namespace Walletify.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
