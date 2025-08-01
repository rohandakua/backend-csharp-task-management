using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TimeTracking;
using PropVivo.Application.Features.TimeTracking.StartTimeTracking;
using PropVivo.Application.Features.TimeTracking.StopTimeTracking;
using PropVivo.Application.Features.TimeTracking.PauseTimeTracking;
using PropVivo.Application.Features.TimeTracking.ResumeTimeTracking;
using PropVivo.Application.Features.TimeTracking.GetTimeTracking;
using PropVivo.Application.Features.TimeTracking.GetTimeTrackingHistory;
using PropVivo.Application.Features.TimeTracking.GetCurrentTimeTracking;
using PropVivo.Application.Features.TimeTracking.GetTodayTimeTracking;

namespace PropVivo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TimeTrackingController : BaseController
    {
        [HttpPost("start")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<TimeTrackingResponse>>> StartTimeTracking([FromBody] StartTimeTrackingRequest request)
        {
            var command = new StartTimeTrackingCommand
            {
                TaskId = request.TaskId,
                UserId = GetCurrentUserId()
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("stop")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<bool>>> StopTimeTracking()
        {
            var command = new StopTimeTrackingCommand { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("pause")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<bool>>> PauseTimeTracking()
        {
            var command = new PauseTimeTrackingCommand { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("resume")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<bool>>> ResumeTimeTracking()
        {
            var command = new ResumeTimeTrackingCommand { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("current")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<TimeTrackingResponse>>> GetCurrentTimeTracking()
        {
            var query = new GetCurrentTimeTrackingQuery { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<ActionResult<BaseResponse<List<TimeTrackingResponse>>>> GetTimeTrackingHistory([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetTimeTrackingHistoryQuery
            {
                UserId = GetCurrentUserId(),
                StartDate = startDate ?? DateTime.Today.AddDays(-30),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<ActionResult<BaseResponse<TimeTrackingResponse>>> GetTodayTimeTracking()
        {
            var query = new GetTodayTimeTrackingQuery { UserId = GetCurrentUserId() };
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
} 