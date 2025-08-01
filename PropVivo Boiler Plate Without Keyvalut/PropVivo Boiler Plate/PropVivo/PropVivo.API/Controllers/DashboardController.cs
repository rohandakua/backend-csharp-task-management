using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Dashboard;
using PropVivo.Application.Features.Dashboard.GetDashboard;
using PropVivo.Application.Features.Dashboard.GetSuperiorDashboard;
using PropVivo.Application.Features.Dashboard.GetStats;

namespace PropVivo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponse<DashboardResponse>>> GetDashboard()
        {
            var query = new GetDashboardQuery { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("superior")]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<DashboardResponse>>> GetSuperiorDashboard()
        {
            var query = new GetSuperiorDashboardQuery { SuperiorId = GetCurrentUserId() };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<BaseResponse<DashboardStats>>> GetStats([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetStatsQuery 
            { 
                UserId = GetCurrentUserId(),
                StartDate = startDate ?? DateTime.Today.AddDays(-7),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
} 