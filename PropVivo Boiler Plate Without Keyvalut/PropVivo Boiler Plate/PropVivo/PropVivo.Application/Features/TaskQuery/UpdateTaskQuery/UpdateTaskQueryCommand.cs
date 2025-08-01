using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Features.TaskQuery.UpdateTaskQuery
{
    public class UpdateTaskQueryCommand : IRequest<BaseResponse<TaskQueryResponse>>
    {
        public string Id { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public QueryStatus Status { get; set; } = QueryStatus.Open;
        public QueryPriority Priority { get; set; } = QueryPriority.Medium;
        public string? AssignedToId { get; set; }
        public string? Resolution { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
    }
} 