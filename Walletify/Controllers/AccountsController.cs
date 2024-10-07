using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Walletify.Models.Entities;
using Walletify.ViewModel.Identity;
using Walletify.ViewModel.Accounts;
using NuGet.Protocol.Core.Types;
using Walletify.Repositories.Interfaces;
namespace Walletify.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepositoryFactory _repository;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IRepositoryFactory repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
        }
        [HttpGet]
        public IActionResult UpdateAccount()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var account = _repository.Account.FindByCondition(a => a.UserId == userId).First();
                var accountViewModel = new UpdateAccountViewModel
                {
                    SavedAmountPerMonth = account.SavedAmountPerMonth,
                    SavingTargetAmount = account.SavingTargetAmount,
                };
                return View(accountViewModel);
            }
                return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateAccount(UpdateAccountViewModel model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                if(ModelState.IsValid)
                {
                    var account = _repository.Account.FindByCondition(a => a.UserId == userId).First();
                    account.SavedAmountPerMonth = model.SavedAmountPerMonth;
                    account.SavingTargetAmount = model.SavingTargetAmount;
                    _repository.Account.Update(account);
                    _repository.Save();
                    return RedirectToAction("Index", "Dashboard");
                }
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }
    }
}