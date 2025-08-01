using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Auth;

namespace PropVivo.Application.Features.Auth.Login
{
    public class LoginCommand : IRequest<BaseResponse<LoginResponse>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 