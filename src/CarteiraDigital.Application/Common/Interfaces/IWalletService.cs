using CarteiraDigital.Application.Features.DTOs.Wallet;

namespace CarteiraDigital.Application.Common.Interfaces
{
    public interface IWalletService
    {
        Task<decimal> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
        Task DepositAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default);
        Task TransferAsync(Guid fromUserId, Guid toUserId, decimal amount, string? description = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TransactionHistoryDto>> GetTransactionHistoryAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    }
}
