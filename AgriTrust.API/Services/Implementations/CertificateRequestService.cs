using AgriTrust.API.Data;
using AgriTrust.API.DTOs.CertificateRequest;
using AgriTrust.API.Models;
using AgriTrust.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgriTrust.API.Services.Implementations
{
    public class CertificateRequestService : ICertificateRequestService
    {
        private readonly AgriTrustDbContext _context;

        public CertificateRequestService(AgriTrustDbContext context)
        {
            _context = context;
        }

        public async Task<CertificateRequestResponseDto> CreateRequestAsync(CreateCertificateRequestDto dto)
        {
            var farmer = await _context.Users.FindAsync(dto.FarmerId);

            if (farmer == null || farmer.Role != "Farmer")
                throw new Exception("Farmer not found");

            var request = new CertificateRequest
            {
                FarmerId = dto.FarmerId,
                Status = "Pending"
            };

            _context.CertificateRequests.Add(request);
            await _context.SaveChangesAsync();

            return new CertificateRequestResponseDto
            {
                Id = request.Id,
                FarmerId = farmer.Id,
                FarmerName = farmer.FullName,
                WalletAddress = farmer.WalletAddress,
                Status = request.Status,
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
                    Status = r.Status,
                    RequestedAt = r.RequestedAt
                })
                .ToListAsync();
        }

        public async Task<CertificateRequestResponseDto> UpdateStatusAsync(int requestId, string status)
        {
            var request = await _context.CertificateRequests
                .Include(r => r.Farmer)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new Exception("Request not found");

            request.Status = status;

            await _context.SaveChangesAsync();

            return new CertificateRequestResponseDto
            {
                Id = request.Id,
                FarmerId = request.FarmerId,
                FarmerName = request.Farmer.FullName,
                WalletAddress = request.Farmer.WalletAddress,
                Status = request.Status,
                RequestedAt = request.RequestedAt
            };
        }
    }
}