using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Dto.Break;
using PropVivo.Application.Features.Break.StartBreak;
using PropVivo.Application.Features.Break.EndBreak;
using PropVivo.Application.Features.Break.GetBreakHistory;
using PropVivo.Application.Features.Break.GetTodayBreaks;
using PropVivo.Application.Common.Base;

namespace PropVivo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee")]
    public class BreakController : BaseController
    {
        [HttpPost("start")]
        public async Task<ActionResult<BaseResponse<bool>>> StartBreak([FromBody] StartBreakRequest request)
        {
            var command = new StartBreakCommand
            {
                UserId = GetCurrentUserId(),
                Type = request.Type,
                Reason = request.Reason
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("end")]
        public async Task<ActionResult<BaseResponse<bool>>> EndBreak()
        {
            var command = new EndBreakCommand { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<ActionResult<BaseResponse<List<BreakResponse>>>> GetBreakHistory([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetBreakHistoryQuery
            {
                UserId = GetCurrentUserId(),
                StartDate = startDate ?? DateTime.Today.AddDays(-7),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<ActionResult<BaseResponse<List<BreakResponse>>>> GetTodayBreaks()
        {
            var query = new GetTodayBreaksQuery { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
} 