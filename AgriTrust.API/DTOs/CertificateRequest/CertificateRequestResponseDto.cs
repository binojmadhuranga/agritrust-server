namespace AgriTrust.API.DTOs.CertificateRequest
{
    public class CertificateRequestResponseDto
    {
        public int Id { get; set; }

        public int FarmerId { get; set; }

        public string FarmerName { get; set; } = string.Empty;

        public string? WalletAddress { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; }
    }
}