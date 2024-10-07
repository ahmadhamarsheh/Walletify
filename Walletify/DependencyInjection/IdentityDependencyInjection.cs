using Microsoft.AspNetCore.Identity;
using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;

namespace Walletify.DependencyInjection
{
    public static class IdentityDependencyInjection
    {
        public static IServiceCollection AddIdentityDependencyInjection(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                //password settings
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 10;
                opt.Password.RequireNonAlphanumeric = true;
                //lockout settings
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 5;

                //user setting
                opt.User.RequireUniqueEmail = true;
                //signin settings
                opt.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            return services;
        }
    }
}
