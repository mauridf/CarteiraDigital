using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CarteiraDigital.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Users.Any())
                return;

            var users = new List<User>
            {
                new("Admin", "User", "admin@example.com", BCrypt.Net.BCrypt.HashPassword("Admin@123")),
                new("John", "Doe", "john.doe@example.com", BCrypt.Net.BCrypt.HashPassword("John@123")),
                new("Jane", "Smith", "jane.smith@example.com", BCrypt.Net.BCrypt.HashPassword("Jane@123"))
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Adicionar depósitos iniciais
            var walletService = serviceProvider.GetRequiredService<IWalletService>();
            foreach (var user in users)
            {
                await walletService.DepositAsync(user.Id, 1000);
            }
        }
    }
}
