using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AgriTrust.API.Data;
using AgriTrust.API.DTOs.Auth;
using AgriTrust.API.Models;
using AgriTrust.API.Helpers;

namespace AgriTrust.API.Services.Auth
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
                throw new Exception("User already exists");

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email
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
                throw new Exception("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials");

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }
    }
}
