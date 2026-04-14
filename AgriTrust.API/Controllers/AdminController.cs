using AgriTrust.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgriTrust.API.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _adminService.GetAllUsersAsync());
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _adminService.DeleteUserAsync(id);

        return Ok(new
        {
            message = "User deleted successfully"
        });
    }
}