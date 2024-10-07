using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Walletify.Repositories.Implementation;

namespace Walletify.DependencyInjection
{
    public static class EmailDependancyInjection
    {
        public static IServiceCollection AddEmailSenderDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();
          
            return services;
        }
    }
}
