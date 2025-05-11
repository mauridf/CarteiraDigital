using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Repositories;
using CarteiraDigital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Transactions
                .Include(t => t.Wallet)
                .Include(t => t.RelatedWallet)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Transaction>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Transactions
                .Include(t => t.Wallet)
                .Include(t => t.RelatedWallet)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Transaction entity, CancellationToken cancellationToken = default)
        {
            await _context.Transactions.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Transaction entity, CancellationToken cancellationToken = default)
        {
            _context.Transactions.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Transaction entity, CancellationToken cancellationToken = default)
        {
            _context.Transactions.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Transaction>> GetByWalletIdAsync(
            Guid walletId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Transactions
                .Include(t => t.Wallet)
                    .ThenInclude(w => w.User)
                .Include(t => t.RelatedWallet)
                    .ThenInclude(w => w.User)
                .Where(t => t.Wallet.Id == walletId);

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedOn >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CreatedOn <= endDate.Value);

            return await query
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync(cancellationToken);
        }
    }
}
