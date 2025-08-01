using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;

namespace PropVivo.Application.Features.TaskQuery.GetTaskQueryById
{
    public class GetTaskQueryByIdQuery : IRequest<BaseResponse<TaskQueryResponse>>
    {
        public string Id { get; set; } = string.Empty;
    }
} 