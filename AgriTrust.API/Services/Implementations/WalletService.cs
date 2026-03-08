using AgriTrust.API.Data;
using AgriTrust.API.Exceptions;
using AgriTrust.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgriTrust.API.Services.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly AgriTrustDbContext _context;

        public WalletService(AgriTrustDbContext context)
        {
            _context = context;
        }

        public async Task ConnectWalletAsync(int userId, string role, string walletAddress)
        {
            if (role != "Farmer")
                throw new ForbiddenException("Only farmers can connect a wallet");

            var user = await _context.Users.FindAsync(userId)
                ?? throw new NotFoundException("User not found");

            if (!string.IsNullOrEmpty(user.WalletAddress))
                throw new ConflictException("Wallet already connected");

            bool walletInUse = await _context.Users
                .AnyAsync(u => u.WalletAddress == walletAddress);

            if (walletInUse)
                throw new ConflictException("Wallet address already linked to another account");

            user.WalletAddress = walletAddress;
            await _context.SaveChangesAsync();
        }


        public async Task<string> GetWalletAddressAsync(int userId, string role)
        {
            if (role != "Farmer")
                throw new ForbiddenException("Only farmers can access wallet details");

            var user = await _context.Users.FindAsync(userId)
                ?? throw new NotFoundException("User not found");

            if (string.IsNullOrEmpty(user.WalletAddress))
                throw new ConflictException("Wallet not connected");

            return user.WalletAddress;
        }

    }
}