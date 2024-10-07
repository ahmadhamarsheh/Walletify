using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;
using Walletify.ViewModel.Transactions;

namespace Walletify.Controllers
{
   [Authorize]
   public class TransactionController : Controller
    {
        
        private readonly IRepositoryFactory _repository;
        public TransactionController(IRepositoryFactory repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Spending()
        {
            var outcomeCategories = _repository.Category
            .FindByCondition(c => c.CategoryType == CategoryType.Spending)
            .ToList();
            ViewBag.outcomeCategories = outcomeCategories;
            return View();
        }
        [HttpPost]
        public IActionResult Spending(TransactionViewModel model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {

                var account = _repository.Account.FindByCondition(a => a.UserId == userId).FirstOrDefault();
                if (account == null )
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(model); // Return the view with the current ViewModel to show error
                }

                var notAllowSpending = ((account.Balance == 0) || ((account.Balance - model.Amount) < 0));

                if (notAllowSpending)
                {
                    //ALERT
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var transaction = new Transaction
                    {
                        UserId = userId,
                        Amount = model.Amount,
                        Note = model.Note,
                        CategoryId = model.CategoryId,
                        TransationType = TransationType.Spending,
                        TransactionDate = DateTime.Now
                    };

                    account.Balance -= model.Amount;
                    try
                    {
                        // Transaction and account update
                        _repository.Transaction.Create(transaction);
                        _repository.Account.Update(account);
                        _repository.Save();
                        return RedirectToAction("Index", "Dashboard");
                    }
                    catch (Exception ex)
                    {
                        // Log the error and handle failure case
                        ModelState.AddModelError("", "Transaction failed to save.");
                    }
                }
                var outcomeCategories = _repository.Category
                  .FindByCondition(c => c.CategoryType == CategoryType.Spending)
                  .ToList();


                ViewBag.outcomeCategories = outcomeCategories;

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Invalid user ID.");
                return Content("OK");
            }
            
        }

        [HttpGet]
        public IActionResult Income()
        {
            var incomeCategories = _repository.Category
            .FindByCondition(c => c.CategoryType == CategoryType.Income)
            .ToList();


            ViewBag.incomeCategories = incomeCategories;
            return View();
        }

        [HttpPost]
        public IActionResult Income(TransactionViewModel model)
        {
            
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {

                var account = _repository.Account.FindByCondition(a => a.UserId == userId).FirstOrDefault();
                if (account == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(model); // Return the view with the current ViewModel to show error
                }
                if (ModelState.IsValid)
                {
                    var transaction = new Transaction
                    {
                        UserId = userId,
                        Amount = model.Amount,
                        Note = model.Note,
                        CategoryId = model.CategoryId,
                        TransationType = TransationType.Income,
                        TransactionDate = DateTime.Now
                    };

                    account.Balance +=  model.Amount;

                    try
                    {
                        // Transaction and account update
                        _repository.Transaction.Create(transaction);
                        _repository.Account.Update(account);
                        _repository.Save();
                        return RedirectToAction("Index", "Dashboard");
                    }
                    catch (Exception ex)
                    {
                        // Log the error and handle failure case
                        ModelState.AddModelError("", "Transaction failed to save.");
                    }

                }
                var incomeCategories = _repository.Category
                  .FindByCondition(c => c.CategoryType == CategoryType.Income)
                  .ToList();


                ViewBag.incomeCategories = incomeCategories;

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Invalid user ID.");
                return Content("Ok");
            }
            
        }
    }
}
