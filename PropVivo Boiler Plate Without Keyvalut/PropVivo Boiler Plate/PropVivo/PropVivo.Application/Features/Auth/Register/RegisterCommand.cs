using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Auth;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Features.Auth.Register
{
    public class RegisterCommand : IRequest<BaseResponse<LoginResponse>>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? SuperiorId { get; set; }
    }
} 