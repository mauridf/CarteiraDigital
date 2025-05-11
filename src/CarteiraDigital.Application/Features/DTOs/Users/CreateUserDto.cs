namespace CarteiraDigital.Application.Features.DTOs.Users
{
    public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Password);
}
