using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;

namespace PropVivo.Application.Features.TaskQuery.ResolveTaskQuery
{
    public class ResolveTaskQueryCommand : IRequest<BaseResponse<TaskQueryResponse>>
    {
        public string Id { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
        public string ResolvedById { get; set; } = string.Empty;
    }
}