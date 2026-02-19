using System.Text.Json.Serialization;

namespace AgriTrust.API.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Role { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
