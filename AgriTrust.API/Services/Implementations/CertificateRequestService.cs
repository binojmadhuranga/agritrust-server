using AgriTrust.API.Data;
using AgriTrust.API.DTOs.CertificateRequest;
using AgriTrust.API.Enums;
using AgriTrust.API.Exceptions;
using AgriTrust.API.Models;
using AgriTrust.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AgriTrust.API.Helpers;

namespace AgriTrust.API.Services.Implementations;

    public class CertificateRequestService : ICertificateRequestService
    {
        private readonly AgriTrustDbContext _context;
        private readonly IBlockchainService _blockchainService;

        public CertificateRequestService(AgriTrustDbContext context, IBlockchainService blockchainService )
        {
            _context = context;
            _blockchainService = blockchainService;
        }

        public async Task<CertificateRequestResponseDto> CreateRequestAsync(int farmerId)
        {
            var farmer = await _context.Users.FindAsync(farmerId);

            if (farmer == null || farmer.Role != "Farmer")
                throw new NotFoundException("Farmer not found");

            var existingPending = await _context.CertificateRequests
                .FirstOrDefaultAsync(r =>
                    r.FarmerId == farmerId &&
                    r.Status == CertificateRequestStatus.Pending);

            if (existingPending != null)
                throw new ConflictException("You already have a pending request");

            var request = new CertificateRequest
            {
                FarmerId = farmerId,
                Status = CertificateRequestStatus.Pending
            };

            _context.CertificateRequests.Add(request);
            await _context.SaveChangesAsync();

            return new CertificateRequestResponseDto
            {
                Id = request.Id,
                FarmerId = farmer.Id,
                FarmerName = farmer.FullName,
                WalletAddress = farmer.WalletAddress,
                Status = request.Status.ToString(),
                RequestedAt = request.RequestedAt
            };
        }

        public async Task<List<CertificateRequestResponseDto>> GetAllRequestsAsync()
        {
            return await _context.CertificateRequests
                .Include(r => r.Farmer)
                .Select(r => new CertificateRequestResponseDto
                {
                    Id = r.Id,
                    FarmerId = r.FarmerId,
                    FarmerName = r.Farmer.FullName,
                    WalletAddress = r.Farmer.WalletAddress,
                    Status = r.Status.ToString(),
                    RequestedAt = r.RequestedAt
                })
                .ToListAsync();
        }

        public async Task<CertificateRequestResponseDto> UpdateStatusAsync(int requestId, CertificateRequestStatus status)
        {
            var request = await _context.CertificateRequests
                .Include(r => r.Farmer)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new NotFoundException("Request not found");

            request.Status = status;

            if (status == CertificateRequestStatus.Approved)
            {
                var certificateNumber = $"CERT-{DateTime.UtcNow.Ticks}";

                var hashInput = $"{certificateNumber}-{request.FarmerId}-{DateTime.UtcNow}";

                var hash = HashHelper.GenerateHash(hashInput);

                var certificate = new Certificate
                {
                    CertificateNumber = certificateNumber,
                    FarmerId = request.FarmerId,
                    IssuedAt = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Hash = hash
                };

                _context.Set<Certificate>().Add(certificate);

                await _blockchainService.StoreCertificateHashAsync(
                    certificate.CertificateNumber,
                    certificate.Hash
                );
            }

            await _context.SaveChangesAsync();

            return new CertificateRequestResponseDto
            {
                Id = request.Id,
                FarmerId = request.FarmerId,
                FarmerName = request.Farmer.FullName,
                WalletAddress = request.Farmer.WalletAddress,
                Status = request.Status.ToString(),
                RequestedAt = request.RequestedAt
            };
        }
    }
