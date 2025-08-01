using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;

namespace PropVivo.Application.Features.TaskQuery.GetAllTaskQueries
{
    public class GetAllTaskQueriesQuery : IRequest<BaseResponse<List<TaskQueryResponse>>>
    {
        public string? RaisedById { get; set; }
        public string? AssignedToId { get; set; }
        public string? TaskId { get; set; }
    }
} 