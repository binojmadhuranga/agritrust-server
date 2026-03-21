using AgriTrust.API.Services.Interfaces;
namespace AgriTrust.API.Services.Implementations;

public class BlockchainService : IBlockchainService
{
    public Task StoreCertificateHashAsync(string certificateNumber, string hash)
    {
        Console.WriteLine($"Blockchain stored: {certificateNumber} - {hash}");
        return Task.CompletedTask;
    }
}