namespace AgriTrust.API.Services.Implementations;
using AgriTrust.API.Services.Interfaces;

public class BlockchainService : IBlockchainService
{
    public Task StoreCertificateHashAsync(string certificateNumber, string hash)
    {
        Console.WriteLine($"Blockchain stored: {certificateNumber} - {hash}");
        return Task.CompletedTask;
    }
}