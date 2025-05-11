using CarteiraDigital.Domain.Common;
using CarteiraDigital.Domain.Entities;

namespace CarteiraDigital.Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IReadOnlyList<Transaction>> GetByWalletIdAsync(
            Guid walletId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
    }
}
