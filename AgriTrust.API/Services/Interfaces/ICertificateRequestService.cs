using AgriTrust.API.DTOs.CertificateRequest;

namespace AgriTrust.API.Services.Interfaces
{
    public interface ICertificateRequestService
    {
        Task<CertificateRequestResponseDto> CreateRequestAsync(CreateCertificateRequestDto dto);

        Task<List<CertificateRequestResponseDto>> GetAllRequestsAsync();

        Task<CertificateRequestResponseDto> UpdateStatusAsync(int requestId, string status);
    }
}