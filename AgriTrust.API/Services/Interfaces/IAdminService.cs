using AgriTrust.API.DTOs.Admin;

namespace AgriTrust.API.Services.Interfaces;

public interface IAdminService
{
    Task<List<UserListDto>> GetAllUsersAsync();

    Task DeleteUserAsync(int userId);
}