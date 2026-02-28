using AgriTrust.API.Data;
using AgriTrust.API.DTOs.Auth;
using AgriTrust.API.Exceptions;
using AgriTrust.API.Helpers;
using AgriTrust.API.Models;
using AgriTrust.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgriTrust.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AgriTrustDbContext _context;
        private readonly JwtTokenHelper _jwtHelper;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(AgriTrustDbContext context, JwtTokenHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new AuthException("Email already registered", 409);

          
            var allowedRoles = new List<string> { "User", "Farmer", "Vendor" };

            
            var role = "User";

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                if (!allowedRoles.Contains(request.Role))
                    throw new AuthException("Invalid role selected", 400);

                role = request.Role;
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Role = role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }


        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new AuthException("Invalid email or password", 401);

            var result = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new AuthException("Invalid email or password", 401);

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                Role = user.Role
            };
        }
    }
}
