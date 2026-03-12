using System.Security.Claims;
using AgriTrust.API.DTOs.CertificateRequest;
using AgriTrust.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriTrust.API.Controllers
{
    [ApiController]
    [Route("api/certificate-requests")]
    [Authorize]
    public class CertificateRequestController : ControllerBase
    {
        private readonly ICertificateRequestService _service;

        public CertificateRequestController(ICertificateRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCertificateRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            int farmerId = int.Parse(userIdClaim);

            var result = await _service.CreateRequestAsync(farmerId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllRequestsAsync();
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateCertificateRequestStatusDto dto)
        {
            var result = await _service.UpdateStatusAsync(id, dto.Status);
            return Ok(result);
        }
    }
}