using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Walletify.ApplicationDbContext;
using Walletify.Controllers;
using Walletify.DependencyInjection;
using Walletify.Repositories.Implementation;
using Walletify.Repositories.Interfaces;
using Walletify.ViewModel;
namespace Walletify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            // Configure Factory
            builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>()
                .AddIdentityDependencyInjection();
            // Add auto mapper
            builder.Services.AddAutoMapper(typeof(Program), typeof(MappingProfile));
            // Add Hangfire services
            builder.Services.AddHangfire(config =>
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Add the Hangfire server
            builder.Services.AddHangfireServer();
            var app = builder.Build();
            // Enable Hangfire Dashboard
            app.UseHangfireDashboard();

            // Map routes for Hangfire Dashboard (optional: secured access can be configured here)
            app.MapHangfireDashboard();
            // Example of a recurring job
            RecurringJob.AddOrUpdate<SavingController>(
                "UpdateSavingTargetAmount",
                   service => service.UpdateSavingTargetAmount(),
                  "*/3 * * * *"
            //Cron.Monthly // Run this on the 1st of every month
            );
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
