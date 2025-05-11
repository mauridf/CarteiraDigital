namespace CarteiraDigital.Application.Features.DTOs.Wallet
{
    public record TransferDto(
    Guid FromUserId,
    Guid ToUserId,
    decimal Amount,
    string? Description = null);
}
