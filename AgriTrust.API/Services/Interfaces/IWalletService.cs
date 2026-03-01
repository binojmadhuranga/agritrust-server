namespace AgriTrust.API.Services.Interfaces
{
    public interface IWalletService
    {
        Task ConnectWalletAsync(int userId, string role, string walletAddress);
        Task<string> GetWalletAddressAsync(int userId, string role);
    }
}
