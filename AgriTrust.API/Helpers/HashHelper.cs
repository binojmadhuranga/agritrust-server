namespace AgriTrust.API.Helpers;
using System.Security.Cryptography;
using System.Text;

public static class HashHelper
{
    public static string GenerateHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}