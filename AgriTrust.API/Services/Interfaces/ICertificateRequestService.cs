using AgriTrust.API.DTOs.CertificateRequest;

namespace AgriTrust.API.Services.Interfaces
{
    public interface ICertificateRequestService
    {
        Task<CertificateRequestResponseDto> CreateRequestAsync(int farmerId);
        
        Task<List<CertificateRequestResponseDto>> GetAllRequestsAsync();

        Task<CertificateRequestResponseDto> UpdateStatusAsync(int requestId, string status);
    }
}