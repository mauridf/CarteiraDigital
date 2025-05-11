using CarteiraDigital.Domain.Common;
using CarteiraDigital.Domain.Entities;

namespace CarteiraDigital.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    }
}
