using Microsoft.AspNetCore.Mvc;
using Walletify.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Walletify.ViewModel.Accounts;
using Walletify.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Walletify.ViewModel.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Walletify.Repositories.Implementation;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.EntityFrameworkCore;

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
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "You need to confirm your email to log in.");
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

        private string GenerateConfirmationCode()
        {
            return Guid.NewGuid().ToString(); // أو أي طريقة أخرى لتوليد كود عشوائي
        }

        [HttpGet]
        public IActionResult EnterConfirmationCode()
        {
            return View();
        }

        public IActionResult EmailConfirmed()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string confirmationCode)
        {
            // الحصول على المستخدم الذي له نفس رمز التأكيد
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.ConfirmationCode == confirmationCode);

            if (user != null)
            {
                user.EmailConfirmed = true; // تأكيد البريد الإلكتروني
                user.ConfirmationCode = string.Empty; // إلغاء الرمز بعد استخدامه
                await _userManager.UpdateAsync(user);
                return RedirectToAction("FinancialInformation", new { id = user.Id }); // إعادة توجيه إلى صفحة التأكيد

            }

            ModelState.AddModelError("", "Invalid confirmation code.");
            return View("EnterConfirmationCode"); // عرض النموذج مرة أخرى مع رسالة خطأ
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // توليد رمز تأكيد بسيط
                    user.ConfirmationCode = Guid.NewGuid().ToString();

                    // تحديث المستخدم في قاعدة البيانات
                    await _userManager.UpdateAsync(user);

                    // تمرير الرمز إلى TempData
                    TempData["ConfirmationCode"] = user.ConfirmationCode;

                    // إعادة توجيه المستخدم إلى صفحة إدخال رمز التأكيد
                    return RedirectToAction("EnterConfirmationCode", "Authentication");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Authentication");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToAction("ForgetPassword");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Accounts", new { token, email = user.Email }, Request.Scheme);

                // هنا يمكن إرسال البريد الإلكتروني إذا كنت بحاجة لذلك
                // await _emailSender.SendEmailAsync(model.Email, "Reset Password", resetLink);

                return RedirectToAction("Index", "Dashboard");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgetPassword");
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ForgetPassword");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult FinancialInformation(string id)
        {
            ViewBag.UserId = id; // تعيين UserId في ViewBag
            return View(new AccountViewModel { UserId = id }); // إرسال UserId إلى الـ ViewModel
        }

        [HttpPost]
        public IActionResult FinancialInformation(AccountViewModel model)
        {
            // تعيين UserId من ViewBag إذا كان null
            if (string.IsNullOrEmpty(model.UserId))
            {
                model.UserId = ViewBag.UserId as string;
            }

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

            // إعادة تعيين UserId في ViewBag إذا كانت ModelState غير صحيحة
            ViewBag.UserId = model.UserId;
            return View(model);
        }
    }
}
