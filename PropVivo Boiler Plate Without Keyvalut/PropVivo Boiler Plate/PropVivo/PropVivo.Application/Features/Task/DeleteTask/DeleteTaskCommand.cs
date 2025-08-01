using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Features.Task.DeleteTask
{
    public class DeleteTaskCommand : IRequest<BaseResponse<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
} 