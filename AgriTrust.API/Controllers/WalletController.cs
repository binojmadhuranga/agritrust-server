using AgriTrust.API.DTOs.Wallet;
using AgriTrust.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int userId = int.Parse(userIdClaim.Value);


            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null)
                return Unauthorized("Invalid token");

            string role = roleClaim.Value;

            await _walletService.ConnectWalletAsync(
                userId,
                role,
                dto.WalletAddress
            );

            return Ok(new { message = "Wallet connected successfully" });
        }
    }
}