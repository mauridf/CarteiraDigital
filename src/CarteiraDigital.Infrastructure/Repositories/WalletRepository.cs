using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Repositories;
using CarteiraDigital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;

        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Wallet entity, CancellationToken cancellationToken = default)
        {
            await _context.Wallets.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Wallet entity, CancellationToken cancellationToken = default)
        {
            _context.Wallets.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Wallet entity, CancellationToken cancellationToken = default)
        {
            _context.Wallets.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .Include(w => w.User)
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.User.Id == userId, cancellationToken);
        }
    }
}
