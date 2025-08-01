using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Features.TaskQuery.CreateTaskQuery
{
    public class CreateTaskQueryCommand : IRequest<BaseResponse<TaskQueryResponse>>
    {
        public string TaskId { get; set; } = string.Empty;
        public string RaisedById { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public QueryPriority Priority { get; set; } = QueryPriority.Medium;
        public List<string> Attachments { get; set; } = new List<string>();
    }
} 