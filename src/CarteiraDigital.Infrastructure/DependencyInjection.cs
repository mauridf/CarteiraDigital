using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Application.Common.Services;
using CarteiraDigital.Domain.Repositories;
using CarteiraDigital.Infrastructure.Data;
using CarteiraDigital.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarteiraDigital.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IWalletService, WalletService>();

            return services;
        }
    }
}
