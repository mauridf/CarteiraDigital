using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Application.Features.DTOs.Wallet;
using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Repositories;

namespace CarteiraDigital.Application.Common.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public WalletService(
            IUserRepository userRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<decimal> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var wallet = await GetWalletByUserIdAsync(userId, cancellationToken);
            return wallet.Balance;
        }

        public async Task DepositAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default)
        {
            var wallet = await GetWalletByUserIdAsync(userId, cancellationToken);

            var transaction = Transaction.Create(
                wallet.Id,
                amount,
                TransactionType.Deposit,
                "Deposit to wallet");

            try
            {
                wallet.Deposit(amount);
                transaction.Complete();

                await _walletRepository.UpdateAsync(wallet, cancellationToken);
                await _transactionRepository.AddAsync(transaction, cancellationToken);
            }
            catch
            {
                transaction.Fail();
                await _transactionRepository.AddAsync(transaction, cancellationToken);
                throw;
            }
        }

        public async Task TransferAsync(
            Guid fromUserId,
            Guid toUserId,
            decimal amount,
            string? description = null,
            CancellationToken cancellationToken = default)
        {
            if (fromUserId == toUserId)
                throw new ArgumentException("Cannot transfer to yourself.");

            var fromWallet = await GetWalletByUserIdAsync(fromUserId, cancellationToken);
            var toUser = await _userRepository.GetByIdAsync(toUserId, cancellationToken)
                ?? throw new ArgumentException("Recipient user not found.");
            var toWallet = await GetWalletByUserIdAsync(toUserId, cancellationToken);

            var sendTransaction = Transaction.Create(
                fromWallet.Id,
                amount,
                TransactionType.Transfer,
                description,
                toWallet.Id);

            var receiveTransaction = Transaction.Create(
                toWallet.Id,
                amount,
                TransactionType.Transfer,
                description,
                fromWallet.Id);

            try
            {
                fromWallet.Withdraw(amount);
                toWallet.Deposit(amount);

                sendTransaction.Complete();
                receiveTransaction.Complete();

                await _walletRepository.UpdateAsync(fromWallet, cancellationToken);
                await _walletRepository.UpdateAsync(toWallet, cancellationToken);

                await _transactionRepository.AddAsync(sendTransaction, cancellationToken);
                await _transactionRepository.AddAsync(receiveTransaction, cancellationToken);
            }
            catch
            {
                sendTransaction.Fail();
                receiveTransaction.Fail();

                await _transactionRepository.AddAsync(sendTransaction, cancellationToken);
                await _transactionRepository.AddAsync(receiveTransaction, cancellationToken);
                throw;
            }
        }

        public async Task<IReadOnlyList<TransactionHistoryDto>> GetTransactionHistoryAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var wallet = await GetWalletByUserIdAsync(userId, cancellationToken);
            var transactions = await _transactionRepository.GetByWalletIdAsync(
                wallet.Id,
                startDate,
                endDate,
                cancellationToken);

            return transactions.Select(t => new TransactionHistoryDto(
                t.Id,
                t.Amount,
                t.Type.ToString(),
                t.Status.ToString(),
                t.Description,
                t.CreatedOn,
                t.CompletedOn,
                t.RelatedWallet?.User?.FirstName,
                t.RelatedWallet?.User?.LastName)).ToList();
        }

        private async Task<Wallet> GetWalletByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new ArgumentException("User not found.");

            var wallet = await _walletRepository.GetByUserIdAsync(userId, cancellationToken)
                ?? throw new InvalidOperationException("Wallet not found.");

            return wallet;
        }
    }
}