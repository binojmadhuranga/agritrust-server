using AgriTrust.API.DTOs.User;
namespace AgriTrust.API.Services.Interfaces;

public interface IUserService
{
    Task<List<FarmerListDto>> GetFarmersAsync();

    Task<FarmerDetailsDto> GetFarmerByIdAsync(int farmerId);
}