using CarteiraDigital.Application.Features.DTOs.Authentication;
using CarteiraDigital.Domain.Entities;

namespace CarteiraDigital.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenDto> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<User> RegisterAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken = default);
    }
}
