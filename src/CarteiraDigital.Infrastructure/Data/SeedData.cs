using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CarteiraDigital.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Users.Any())
                return;

            var users = new List<User>
        {
            new("Admin", "User", "admin@example.com", BCrypt.Net.BCrypt.HashPassword("Admin@123")),
            new("John", "Doe", "john.doe@example.com", BCrypt.Net.BCrypt.HashPassword("John@123")),
            new("Jane", "Smith", "jane.smith@example.com", BCrypt.Net.BCrypt.HashPassword("Jane@123")),
            new("Carlos", "Silva", "carlos.silva@example.com", BCrypt.Net.BCrypt.HashPassword("Carlos@123")),
            new("Maria", "Oliveira", "maria.oliveira@example.com", BCrypt.Net.BCrypt.HashPassword("Maria@123")),
            new("Lucas", "Souza", "lucas.souza@example.com", BCrypt.Net.BCrypt.HashPassword("Lucas@123")),
            new("Ana", "Lima", "ana.lima@example.com", BCrypt.Net.BCrypt.HashPassword("Ana@123")),
            new("Pedro", "Alves", "pedro.alves@example.com", BCrypt.Net.BCrypt.HashPassword("Pedro@123")),
            new("Fernanda", "Costa", "fernanda.costa@example.com", BCrypt.Net.BCrypt.HashPassword("Fernanda@123")),
            new("Rafael", "Martins", "rafael.martins@example.com", BCrypt.Net.BCrypt.HashPassword("Rafael@123"))

        };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Add initial deposits
            var walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();
            foreach (var user in users)
            {
                await walletService.DepositAsync(user.Id, 1000);
            }
        }
    }
}
