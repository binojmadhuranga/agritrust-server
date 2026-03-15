using AgriTrust.API.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriTrust.API.Data.Seed;

public static class UserSeeder
{
    public static async Task SeedVendorAsync(
        AgriTrustDbContext context,
        IPasswordHasher<User> passwordHasher)
    {
        if (!context.Users.Any(u => u.Email == "vendor@agritrust.com"))
        {
            var vendor = new User
            {
                FullName = "Default Vendor",
                Email = "vendor@agritrust.com",
                Role = "Vendor",
                WalletAddress = null,
                CreatedAt = DateTime.UtcNow
            };

            vendor.PasswordHash = passwordHasher.HashPassword(vendor, "Vendor123@");

            context.Users.Add(vendor);
            await context.SaveChangesAsync();
        }
    }

}