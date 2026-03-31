namespace AgriTrust.API.DTOs.User;

public class FarmerListDto
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public bool IsCertified { get; set; }

    public bool IsBlockchainVerified { get; set; }
}