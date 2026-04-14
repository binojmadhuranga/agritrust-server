namespace AgriTrust.API.DTOs.Admin;

public class UserListDto
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public string? WalletAddress { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsCertified { get; set; }
}