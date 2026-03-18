using AgriTrust.API.Enums;

namespace AgriTrust.API.Models
{
    public class CertificateRequest
    {
        public int Id { get; set; }

        public int FarmerId { get; set; }

        public User Farmer { get; set; } = null!;

        public CertificateRequestStatus Status { get; set; } = CertificateRequestStatus.Pending;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}