namespace AgriTrust.API.DTOs.User;

public class FarmerDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string? WalletAddress { get; set; }

    public bool IsCertified { get; set; }

    public bool IsBlockchainVerified { get; set; }

    public string? CertificateNumber { get; set; }

    public DateTime? IssuedAt { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Hash { get; set; }
}