using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Task;
using PropVivo.Application.Features.Task.CreateTask;
using PropVivo.Application.Features.Task.GetAllTasks;
using PropVivo.Application.Features.Task.GetTaskById;
using PropVivo.Application.Features.Task.UpdateTask;
using PropVivo.Application.Features.Task.DeleteTask;
using PropVivo.Application.Features.Task.StartTask;
using PropVivo.Application.Features.Task.CompleteTask;

namespace PropVivo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<TaskResponse>>>> GetAllTasks([FromQuery] string? assignedToId = null)
        {
            var query = new GetAllTasksQuery { AssignedToId = assignedToId ?? GetCurrentUserId() };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<TaskResponse>>> GetTaskById(string id)
        {
            var query = new GetTaskByIdQuery { Id = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<TaskResponse>>> CreateTask([FromBody] CreateTaskRequest request)
        {
            var command = new CreateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                AssignedToId = request.AssignedToId,
                AssignedById = GetCurrentUserId(),
                EstimatedHours = request.EstimatedHours,
                Priority = request.Priority,
                IsSelfAdded = request.IsSelfAdded
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("self")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<TaskResponse>>> CreateSelfTask([FromBody] CreateTaskRequest request)
        {
            var command = new CreateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                AssignedToId = GetCurrentUserId(),
                AssignedById = GetCurrentUserId(),
                EstimatedHours = request.EstimatedHours,
                Priority = request.Priority,
                IsSelfAdded = true
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<TaskResponse>>> UpdateTask(string id, [FromBody] UpdateTaskRequest request)
        {
            var command = new UpdateTaskCommand
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                EstimatedHours = request.EstimatedHours,
                Priority = request.Priority,
                Status = request.Status
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Superior,Admin")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteTask(string id)
        {
            var command = new DeleteTaskCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/start")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<bool>>> StartTask(string id)
        {
            var command = new StartTaskCommand { TaskId = id, UserId = GetCurrentUserId() };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResponse<bool>>> CompleteTask(string id)
        {
            var command = new CompleteTaskCommand { TaskId = id, UserId = GetCurrentUserId() };
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
} 