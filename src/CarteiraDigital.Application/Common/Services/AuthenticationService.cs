using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Application.Features.DTOs.Authentication;
using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace CarteiraDigital.Application.Common.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<TokenDto> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var token = GenerateJwtToken(user);
            return new TokenDto(token, DateTime.UtcNow.AddDays(7));
        }

        public async Task<User> RegisterAsync(
            string firstName,
            string lastName,
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            if (await _userRepository.EmailExistsAsync(email, cancellationToken))
                throw new ArgumentException("Email already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(firstName, lastName, email, passwordHash);

            await _userRepository.AddAsync(user, cancellationToken);
            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
