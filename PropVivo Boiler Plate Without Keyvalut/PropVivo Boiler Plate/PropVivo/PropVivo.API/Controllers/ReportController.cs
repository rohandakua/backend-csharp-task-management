using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Common;
using PropVivo.Application.Features.Report.GetAttendanceReport;
using PropVivo.Application.Features.Report.GetTaskReport;
using PropVivo.Application.Features.Report.GetTimeTrackingReport;

namespace PropVivo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : BaseController
    {
        [HttpGet("attendance")]
        public async Task<ActionResult<BaseResponse<AttendanceReportResponse>>> GetAttendanceReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetAttendanceReportQuery
            {
                UserId = GetCurrentUserId(),
                StartDate = startDate ?? DateTime.Today.AddDays(-30),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("task")]
        public async Task<ActionResult<BaseResponse<TaskReportResponse>>> GetTaskReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetTaskReportQuery
            {
                UserId = GetCurrentUserId(),
                StartDate = startDate ?? DateTime.Today.AddDays(-30),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("timetracking")]
        public async Task<ActionResult<BaseResponse<TimeTrackingReportResponse>>> GetTimeTrackingReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetTimeTrackingReportQuery
            {
                UserId = GetCurrentUserId(),
                StartDate = startDate ?? DateTime.Today.AddDays(-30),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("superior/employee/{employeeId}")]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<EmployeeReportResponse>>> GetEmployeeReport(string employeeId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetEmployeeReportQuery
            {
                EmployeeId = employeeId,
                StartDate = startDate ?? DateTime.Today.AddDays(-30),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("superior/team")]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<TeamReportResponse>>> GetTeamReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetTeamReportQuery
            {
                SuperiorId = GetCurrentUserId(),
                StartDate = startDate ?? DateTime.Today.AddDays(-30),
                EndDate = endDate ?? DateTime.Today
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
} 