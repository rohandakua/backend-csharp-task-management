using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Auth;
using PropVivo.Application.Features.Auth.Login;
using PropVivo.Application.Features.Auth.Register;

namespace PropVivo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand { Username = request.Username, Password = request.Password };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin,Superior")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand 
            { 
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role,
                SuperiorId = request.SuperiorId
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<bool>>> Logout()
        {
            // Implementation for logout (invalidate token, etc.)
            return Ok(new BaseResponse<bool> { Data = true, Success = true, Message = "Logged out successfully" });
        }
    }
} 