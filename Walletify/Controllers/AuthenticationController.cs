using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Walletify.EmailService;
using Walletify.EmailService.EmailModel;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;
using Walletify.ViewModel.Accounts;
using Walletify.ViewModel.Identity;
using ResetPasswordViewModel = Walletify.EmailService.EmailModel.ResetPasswordViewModel;

namespace Walletify.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepositoryFactory _repository;
        private readonly IEmailSender _emailSender;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IRepositoryFactory repository, EmailService.IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                // Check if the email already exists
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email already exists.");
                    return View(model);
                }

                // Create the user
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
      
                    VerificationCode = RandomCodeGenerator.GenerateRandomCode()
                };

                // Save the user to the database
                var result = await _userManager.CreateAsync(user,model.Password);

                if (result.Succeeded)
                {
                    // Update user with verification code
                    await _userManager.UpdateAsync(user);

                    // Send verification email
                    var emailMessage = EmailMessageGenerator.GenerateEmailMessageForCodeVerification(user.VerificationCode);
                    _emailSender.SendEmailWithCode(user.Email, emailMessage);

                    // Redirect user to the code confirmation page
                    //TempData["ConfirmationCode"] = user.VerificationCode;
                    return RedirectToAction("EnterConfirmationCode", "Authentication", new { email = user.Email });
                }

                // Handle errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult EnterConfirmationCode(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EnterConfirmationCode(EmailViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (model.Code != user.VerificationCode)
                {
                    return RedirectToAction("BadRequest", "Home");
                }
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                _repository.Save();

                return RedirectToAction(nameof(FinancialInformation), new { email = model.Email });

            }
            return RedirectToAction("BadRequest", "Home");

        }
        #endregion 
        #region signin
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
        #endregion

        #region not need
        //public IActionResult EmailConfirmed()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ConfirmEmail(string confirmationCode)
        //{
        //    // الحصول على المستخدم الذي له نفس رمز التأكيد
        //    var user = await _userManager.Users
        //        .FirstOrDefaultAsync(u => u.ConfirmationCode == confirmationCode);

        //    if (user != null)
        //    {
        //        user.EmailConfirmed = true; // تأكيد البريد الإلكتروني
        //        user.ConfirmationCode = string.Empty; // إلغاء الرمز بعد استخدامه
        //        await _userManager.UpdateAsync(user);
        //        return RedirectToAction("FinancialInformation", new { id = user.Id }); // إعادة توجيه إلى صفحة التأكيد

        //    }

        //    ModelState.AddModelError("", "Invalid confirmation code.");
        //    return View("EnterConfirmationCode"); // عرض النموذج مرة أخرى مع رسالة خطأ
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            // توليد رمز تأكيد بسيط
        //            user.ConfirmationCode = Guid.NewGuid().ToString();

        //            // تحديث المستخدم في قاعدة البيانات
        //            await _userManager.UpdateAsync(user);

        //            // تمرير الرمز إلى TempData
        //            TempData["ConfirmationCode"] = user.ConfirmationCode;

        //            // إعادة توجيه المستخدم إلى صفحة إدخال رمز التأكيد
        //            return RedirectToAction("EnterConfirmationCode", "Authentication");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    return View(model);
        //}
        //[HttpGet]
        //public IActionResult RegisterConfirmation()
        //{
        //    return View();
        //}
        #endregion


 

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Authentication");
        }

        //[HttpGet]
        //public IActionResult ForgetPassword()
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult SendEmailToResetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassswordPage(string email)
        {
            ViewBag.Email = email;
            return View("ResetPasssword");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasssword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            user.PasswordCode = RandomCodeGenerator.GenerateRandomCode();
            await _userManager.UpdateAsync(user);
            _repository.Save();
            var emailMessage = EmailMessageGenerator.GenerateEmailMessageForResetPasswordCode(user.PasswordCode);
            _emailSender.SendEmailWithCode(user.Email, emailMessage);
            return RedirectToAction(nameof(ResetPassswordPage), new { email = model.Email });

        }
        [HttpPost]
        public async Task<IActionResult> PerformResetPasssword(ResetPasswordViewModel model) {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user.PasswordCode != model.Code)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            user.PasswordCode = null;
            await _userManager.UpdateAsync(user);
            _repository.Save();

            return RedirectToAction(nameof(SignIn));

        }


        #region password
        //[HttpPost]
        //public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //        {
        //            return RedirectToAction("ForgetPassword");
        //        }

        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var resetLink = Url.Action("ResetPassword", "Accounts", new { token, email = user.Email }, Request.Scheme);

        //        // هنا يمكن إرسال البريد الإلكتروني إذا كنت بحاجة لذلك
        //        // await _emailSender.SendEmailAsync(model.Email, "Reset Password", resetLink);

        //        return RedirectToAction("Index", "Dashboard");
        //    }

        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult ResetPassword(string token, string email)
        //{
        //    if (token == null || email == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var model = new ResetPasswordViewModel { Token = token, Email = email };
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            return RedirectToAction("ForgetPassword");
        //        }

        //        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("ForgetPassword");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    return View(model);
        //}
        #endregion

        #region Financial
        [HttpGet]
        public async Task<IActionResult> FinancialInformation(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
            ViewBag.UserId =user.Id; // تعيين UserId في ViewBag
            return View(); // إرسال UserId إلى الـ ViewModel
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

            // إعادة تعيين UserId في ViewBag إذا كانت ModelState غير صحيحة
            ViewBag.UserId = model.UserId;
            return View(model);
        }
        #endregion
    }
}
