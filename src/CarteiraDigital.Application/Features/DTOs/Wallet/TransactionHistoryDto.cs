namespace CarteiraDigital.Application.Features.DTOs.Wallet
{
    public record TransactionHistoryDto(
    Guid Id,
    decimal Amount,
    string Type,
    string Status,
    string? Description,
    DateTime CreatedOn,
    DateTime? CompletedOn,
    string? RelatedUserFirstName = null,
    string? RelatedUserLastName = null);
}
