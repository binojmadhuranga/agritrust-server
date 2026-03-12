namespace AgriTrust.API.Models
{
    public class CertificateRequest
    {
        public int Id { get; set; }

        public int FarmerId { get; set; }

        public User Farmer { get; set; } = null!;

        public string Status { get; set; } = "Pending";

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}