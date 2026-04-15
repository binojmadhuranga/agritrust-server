using AgriTrust.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AgriTrust.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("farmers")]
    public async Task<IActionResult> GetFarmers()
    {
        return Ok(await _userService.GetFarmersAsync());
    }

    [HttpGet("farmers/{id}")]
    public async Task<IActionResult> GetFarmerById(int id)
    {
        return Ok(await _userService.GetFarmerByIdAsync(id));
    }
}