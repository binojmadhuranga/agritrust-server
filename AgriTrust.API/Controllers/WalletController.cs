using AgriTrust.API.DTOs.Wallet;
using AgriTrust.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriTrust.API.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> ConnectWallet(
            [FromBody] ConnectWalletRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);
            var role = User.FindFirst("role")!.Value;

            await _walletService.ConnectWalletAsync(
                userId,
                role,
                dto.WalletAddress
            );

            return Ok(new { message = "Wallet connected successfully" });
        }
    }
}