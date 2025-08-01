using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Features.TaskQuery.GetSuperiorTaskQueries
{
    public class GetSuperiorTaskQueriesQuery : IRequest<BaseResponse<List<TaskQueryResponse>>>
    {
        public string SuperiorId { get; set; } = string.Empty;
        public QueryStatus? Status { get; set; }
        public QueryPriority? Priority { get; set; }
    }
} 