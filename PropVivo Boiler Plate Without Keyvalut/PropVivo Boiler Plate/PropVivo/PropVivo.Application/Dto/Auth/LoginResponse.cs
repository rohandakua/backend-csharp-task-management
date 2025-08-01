using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? SuperiorId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
} 