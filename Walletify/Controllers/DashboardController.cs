using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;
using Walletify.ViewModel.Dashboard;
using Walletify.ViewModel.Transactions;

namespace Walletify.Controllers
{
    public class DashboardController : Controller
    {
        /* [Authorize] */
        private readonly IRepositoryFactory _repository;
        private readonly IMapper _mapper;

        public DashboardController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var saving = _repository.Saving.FindByCondition(t => t.UserId == userId).FirstOrDefault();
                var totalSavedAmount = saving?.TotalSavedAmount ?? 0;

                var account = _repository.Account.FindByCondition(t => t.UserId == userId).FirstOrDefault();

                if (account == null)
                {
                    return RedirectToAction("FinancialInformation", "Authentication", new { id = userId });
                }

                var savedAmountPerMonth = account.SavedAmountPerMonth != 0 ? account.SavedAmountPerMonth : 0.0m;
                var balance = account.Balance != 0 ? account.Balance : 0.0m;
                var target = account.SavingTargetAmount;

                List<Transaction> topTenTransaction = _repository.Transaction.FindByCondition(t => t.UserId == userId).OrderByDescending(t => t.TransactionDate).ToList();
                IEnumerable<DashboardTransactionViewModel> topTenTransactionViewModel = topTenTransaction.Select(transaction => new DashboardTransactionViewModel
                {
                    TransactionId = transaction.TransationId,
                    Amount = transaction.Amount,
                    Note = transaction.Note ?? "-",
                    TransationType = (transaction.TransationType == 0) ? "Spending" : "Income",
                    CategoryName = _repository.Category.FindByCondition(c => c.Id == transaction.CategoryId).First().Name,
                    TransactionDate = transaction.TransactionDate,
                });

                var dashBordViewModel = new DashboardViewModel
                {
                    TopTransactions = topTenTransactionViewModel,
                    TotalSavedAmount = totalSavedAmount,
                    Balance = balance,
                    SavedAmountPerMonth = savedAmountPerMonth,
                    Ratio = (target == 0) ? 0 : ((int)((totalSavedAmount / target) * 100) >= 100 ? 100 : (int)((totalSavedAmount / target) * 100)),
                };

                return View(dashBordViewModel);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
