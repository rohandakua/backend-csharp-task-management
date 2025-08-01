using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PropVivo.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        private Mediator? _mediator;
        protected Mediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<Mediator>() ?? throw new InvalidOperationException("Mediator service not found");

        protected string GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value ?? string.Empty;
        }
    }
}