using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Application.Features.TaskQuery.CreateTaskQuery;
using PropVivo.Application.Features.TaskQuery.GetAllTaskQueries;
using PropVivo.Application.Features.TaskQuery.GetTaskQueryById;
using PropVivo.Application.Features.TaskQuery.UpdateTaskQuery;
using PropVivo.Application.Features.TaskQuery.ResolveTaskQuery;
using PropVivo.Application.Common.Base;

namespace PropVivo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskQueryController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<TaskQueryResponse>>>> GetAllTaskQueries([FromQuery] string? taskId = null, [FromQuery] string? raisedById = null)
        {
            var query = new GetAllTaskQueriesQuery 
            { 
                TaskId = taskId,
                RaisedById = raisedById ?? GetCurrentUserId()
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("superior")]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<List<TaskQueryResponse>>>> GetSuperiorTaskQueries([FromQuery] string? status = null, [FromQuery] string? priority = null)
        {
            var query = new GetSuperiorTaskQueriesQuery
            {
                SuperiorId = GetCurrentUserId(),
                Status = status,
                Priority = priority
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<TaskQueryResponse>>> GetTaskQueryById(string id)
        {
            var query = new GetTaskQueryByIdQuery { Id = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<TaskQueryResponse>>> CreateTaskQuery([FromBody] CreateTaskQueryRequest request)
        {
            var command = new CreateTaskQueryCommand
            {
                TaskId = request.TaskId,
                RaisedById = GetCurrentUserId(),
                Subject = request.Subject,
                Description = request.Description,
                Priority = request.Priority,
                Attachments = request.Attachments
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<TaskQueryResponse>>> UpdateTaskQuery(string id, [FromBody] UpdateTaskQueryRequest request)
        {
            var command = new UpdateTaskQueryCommand
            {
                Id = id,
                Status = request.Status,
                AssignedToId = request.AssignedToId,
                Priority = request.Priority
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/resolve")]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<TaskQueryResponse>>> ResolveTaskQuery(string id, [FromBody] ResolveTaskQueryRequest request)
        {
            var command = new ResolveTaskQueryCommand
            {
                Id = id,
                ResolvedById = GetCurrentUserId(),
                Resolution = request.Resolution
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<BaseResponse<QueryStatsResponse>>> GetQueryStats()
        {
            // TODO: Implement query stats feature
            var response = new BaseResponse<QueryStatsResponse>
            {
                Data = new QueryStatsResponse(),
                Success = true,
                Message = "Query stats feature not yet implemented"
            };
            return Ok(response);
        }
    }
} 