using AgriTrust.API.Data;
using AgriTrust.API.DTOs.Admin;
using AgriTrust.API.Exceptions;
using AgriTrust.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgriTrust.API.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly AgriTrustDbContext _context;

    public AdminService(AgriTrustDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserListDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.Certificate)
            .ToListAsync();

        return users.Select(u => new UserListDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role,
            WalletAddress = u.WalletAddress,
            CreatedAt = u.CreatedAt,
            IsCertified = u.Certificate != null
        }).ToList();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.Certificate)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new NotFoundException("User not found");

        if (user.Certificate != null)
        {
            _context.Certificates.Remove(user.Certificate);
        }

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }
}