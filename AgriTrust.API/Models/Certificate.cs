namespace AgriTrust.API.Models;

public class Certificate
{
    public int Id { get; set; }

    public string CertificateNumber { get; set; }

    public int FarmerId { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime ExpiryDate { get; set; }

    public string Hash { get; set; }
    
    public User Farmer { get; set; }

}