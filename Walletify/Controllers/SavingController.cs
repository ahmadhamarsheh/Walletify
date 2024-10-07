using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Walletify.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Walletify.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Walletify.Controllers
{

    public class SavingController : Controller
    {
        private readonly IRepositoryFactory _repository;
        private readonly UserManager<ApplicationUser> userManager;

        public SavingController(IRepositoryFactory repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            this.userManager = userManager;
        }
        #region old
        //public void ScheduleMonthlyJobsForAllUsers()
        //{
        //    // Retrieve all users from the repository
        //    var users = _repository.User.FindAll().ToList();

        //    // Iterate through each user and schedule their monthly job
        //    foreach (var user in users)
        //    {
        //        ScheduleMonthlyJob(user);
        //    }

        //}

        //// This method schedules the monthly job
        //public void ScheduleMonthlyJob(User user)
        //{
        //    DateTime createdDate = user.CreationDate;

        //    // Generate a cron expression for running the job on the same day of the month as the CreatedDate
        //    string cronExpression = $"{createdDate.Minute} {createdDate.Hour} {createdDate.Day} * *"; // Runs on the same day each month

        //    // Schedule the recurring job for the user
        //    RecurringJob.AddOrUpdate<SavingController>(
        //        $"UpdateSavingTargetAmount_{user.Id}",  // Unique job identifier per user
        //        service => service.UpdateSavingTargetAmount(),
        //        cronExpression
        //    );

        //}
        #endregion

        public void UpdateSavingTargetAmount()
        {

            var users = userManager.Users.ToList();

                foreach (var user in users)
                {

                    var saving = _repository.Saving.FindByCondition(s => s.UserId == user.Id).FirstOrDefault();
                    if(saving == null)
                    {
                        var save = new Saving { 
                            UserId = user.Id,
                            TotalSavedAmount = 0,
                        };
                        _repository.Saving.Create(save);
                        _repository.Save();
                    }

                    var account = _repository.Account.FindByCondition(s => s.UserId == user.Id).FirstOrDefault();
                    if ( account == null)
                    {
                        continue; // Skip if no saving record is found
                    }

                    var savedAmountPerMonth = account.SavedAmountPerMonth;
                    var targetAmount = account.SavingTargetAmount;
                    // Deduct the available balance and update the saved amount
                    if (account.Balance >= savedAmountPerMonth)
                    {
                        account.Balance -= savedAmountPerMonth;
                        saving.TotalSavedAmount += savedAmountPerMonth;

                    }
                    else
                    {
                    saving.TotalSavedAmount += account.Balance;
                    account.Balance = 0;
                    }
                    _repository.Account.Update(account);
                    _repository.Saving.Update(saving);
                    // Commit all changes to the database
                    _repository.Save();
                }
            }
    }
}
