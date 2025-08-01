using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Task;

namespace PropVivo.Application.Features.Task.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<BaseResponse<TaskResponse>>
    {
        public string Id { get; set; } = string.Empty;
    }
} 