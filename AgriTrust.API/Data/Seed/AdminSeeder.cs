using AgriTrust.API.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriTrust.API.Data.Seed;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(
        AgriTrustDbContext context,
        IPasswordHasher<User> passwordHasher)
    {
        if (!context.Users.Any(u => u.Email == "admin@agritrust.com"))
        {
            var admin = new User
            {
                FullName = "System Admin",
                Email = "admin@agritrust.com",
                Role = "Admin",
                WalletAddress = null,
                CreatedAt = DateTime.UtcNow
            };

            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123@");

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}