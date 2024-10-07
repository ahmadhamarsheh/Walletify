using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using Walletify.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Walletify.ViewModel.Identity;
using Walletify.ViewModel.Accounts;
using Walletify.Repositories.Interfaces;

namespace Walletify.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepositoryFactory _repository;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IRepositoryFactory repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingEmailUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingEmailUser != null)
                {

                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View(model);
                }
                var existingUserNameUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUserNameUser != null)
                {
                    ModelState.AddModelError("UserName", "Username is already taken.");
                    return View(model);
                }
                var newUser = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("FinancialInformation", "Authentication", new { id = newUser.Id });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username or Password is wrong");
                    return View(model);
                }
                var validPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!validPassword)
                {
                    ModelState.AddModelError(string.Empty, "Password is wrong");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "User is not allowed to sign in.");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "User is locked out.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index)); // Redirect to the Signin action
        }

        [HttpGet]
        public IActionResult FinancialInformation(string id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public IActionResult FinancialInformation(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = new Account()
                {
                    UserId = model.UserId,
                    Balance = model.Balance,
                    SavingTargetAmount = model.SavingTargetAmount,
                    SavedAmountPerMonth = model.SavedAmountPerMonth,
                };
                _repository.Account.Create(account);
                _repository.Save();
                return RedirectToAction(nameof(SignIn));
            }
            ViewBag.id = model.UserId;
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
