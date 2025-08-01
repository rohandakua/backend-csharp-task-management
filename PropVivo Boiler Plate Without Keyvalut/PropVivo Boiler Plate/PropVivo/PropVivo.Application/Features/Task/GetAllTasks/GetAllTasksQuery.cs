using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Task;

namespace PropVivo.Application.Features.Task.GetAllTasks
{
    public class GetAllTasksQuery : IRequest<BaseResponse<List<TaskResponse>>>
    {
        public string? AssignedToId { get; set; }
    }
} 