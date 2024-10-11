using Hangfire;
using Microsoft.AspNetCore.Identity;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;

namespace Walletify.Controllers
{

    public class SavingController
    {
        private readonly IRepositoryFactory _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SavingController(IRepositoryFactory repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        // Update Saving Target Amount only for users with confirmed email
        public void UpdateSavingTargetAmountForAllUsers()
        {
            var users = _userManager.Users
                .Where(u => u.EmailConfirmed) // Filter for users with confirmed email
                .ToList();

            foreach (var user in users)
            {
                UpdateSavingTargetAmount(user.Id);
            }
        }


        public void UpdateSavingTargetAmount(string userId)
        {
            var account = _repository.Account.FindByCondition(a => a.UserId == userId).FirstOrDefault();
            var saving = _repository.Saving.FindByCondition(s => s.UserId == userId).FirstOrDefault();

            if (account == null )
            {
                return; // Skip if the job has already run this month or account doesn't exist
            }

            RecurringJob.RemoveIfExists($"CheckBalance-{userId}");

            if (account.Balance >= account.SavedAmountPerMonth)
            {
                DeductAndSave(userId, account, saving);

            }
            else
            {
                ScheduleDailyBalanceCheck(userId, account.SavedAmountPerMonth);
            }

        }

        private void DeductAndSave(string userId, Account account, Saving saving)
        {
            if (saving == null)
            {
                saving = new Saving { UserId = userId, TotalSavedAmount = 0 };
                _repository.Saving.Create(saving);
                _repository.Save();

            }

            account.Balance -= account.SavedAmountPerMonth;
            saving.TotalSavedAmount += account.SavedAmountPerMonth;

            _repository.Account.Update(account);
            _repository.Saving.Update(saving);
            _repository.Save();
        }

        public void ScheduleDailyBalanceCheck(string userId, decimal savedAmountPerMonth)
        {
            RecurringJob.AddOrUpdate($"CheckBalance-{userId}", () => CheckBalance(userId, savedAmountPerMonth),
                Cron.Daily
                );
        }

        public void CheckBalance(string userId, decimal savedAmountPerMonth)
        {
            var account = _repository.Account.FindByCondition(a => a.UserId == userId).FirstOrDefault();
            var saving = _repository.Saving.FindByCondition(s => s.UserId == userId).FirstOrDefault();



            if (account.Balance >= savedAmountPerMonth)
            {
                DeductAndSave(userId, account, saving);
                RecurringJob.RemoveIfExists($"CheckBalance-{userId}");
            }

        }

      

    }
}
