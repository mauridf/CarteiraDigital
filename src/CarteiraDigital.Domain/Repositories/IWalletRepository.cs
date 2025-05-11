using CarteiraDigital.Domain.Common;
using CarteiraDigital.Domain.Entities;

namespace CarteiraDigital.Domain.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
