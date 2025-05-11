using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Application.Features.DTOs.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarteiraDigital.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            var userId = GetCurrentUserId();
            var balance = await _walletService.GetBalanceAsync(userId);
            return Ok(balance);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDto dto)
        {
            var userId = GetCurrentUserId();
            await _walletService.DepositAsync(userId, dto.Amount);
            return NoContent();
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto dto)
        {
            var fromUserId = GetCurrentUserId();
            await _walletService.TransferAsync(fromUserId, dto.ToUserId, dto.Amount, dto.Description);
            return NoContent();
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IReadOnlyList<TransactionHistoryDto>>> GetTransactions(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var userId = GetCurrentUserId();
            var transactions = await _walletService.GetTransactionHistoryAsync(
                userId,
                startDate,
                endDate);

            return Ok(transactions);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid user ID in token.");

            return userId;
        }
    }
}
