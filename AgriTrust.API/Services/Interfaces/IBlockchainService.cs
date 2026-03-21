namespace AgriTrust.API.Services.Interfaces;

public interface IBlockchainService
{
    Task StoreCertificateHashAsync(string certificateNumber, string hash);
}