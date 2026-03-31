using AgriTrust.API.Data;
using AgriTrust.API.DTOs.User;
using AgriTrust.API.Exceptions;
using AgriTrust.API.Helpers;
using AgriTrust.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgriTrust.API.Services.Implementations;

public class UserService : IUserService
{
    private readonly AgriTrustDbContext _context;

    public UserService(AgriTrustDbContext context)
    {
        _context = context;
    }

    public async Task<List<FarmerListDto>> GetFarmersAsync()
    {
        var farmers = await _context.Users
            .Where(u => u.Role == "Farmer")
            .Include(u => u.Certificate)
            .ToListAsync();

        return farmers.Select(f =>
        {
            bool verified = false;

            if (f.Certificate != null)
            {
                var hashInput =
                    $"{f.Certificate.CertificateNumber}|{f.Certificate.FarmerId}|{f.Certificate.IssuedAt:O}|{f.Certificate.ExpiryDate:O}";

                var generatedHash = HashHelper.GenerateHash(hashInput);

                verified = generatedHash == f.Certificate.Hash;
            }

            return new FarmerListDto
            {
                Id = f.Id,
                FullName = f.FullName,
                Email = f.Email,
                IsCertified = f.Certificate != null,
                IsBlockchainVerified = verified
            };
        }).ToList();
    }

    public async Task<FarmerDetailsDto> GetFarmerByIdAsync(int farmerId)
    {
        var farmer = await _context.Users
            .Include(u => u.Certificate)
            .FirstOrDefaultAsync(u => u.Id == farmerId && u.Role == "Farmer");

        if (farmer == null)
            throw new NotFoundException("Farmer not found");

        bool verified = false;

        if (farmer.Certificate != null)
        {
            var hashInput =
                $"{farmer.Certificate.CertificateNumber}|{farmer.Certificate.FarmerId}|{farmer.Certificate.IssuedAt:O}|{farmer.Certificate.ExpiryDate:O}";

            var generatedHash = HashHelper.GenerateHash(hashInput);

            verified = generatedHash == farmer.Certificate.Hash;
        }

        return new FarmerDetailsDto
        {
            Id = farmer.Id,
            FullName = farmer.FullName,
            Email = farmer.Email,
            WalletAddress = farmer.WalletAddress,
            IsCertified = farmer.Certificate != null,
            IsBlockchainVerified = verified,
            CertificateNumber = farmer.Certificate?.CertificateNumber,
            IssuedAt = farmer.Certificate?.IssuedAt,
            ExpiryDate = farmer.Certificate?.ExpiryDate,
            Hash = farmer.Certificate?.Hash
        };
    }
}