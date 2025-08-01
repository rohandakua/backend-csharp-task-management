using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Dto.TaskQuery
{
    public class ResolveTaskQueryRequest
    {
        public string Resolution { get; set; } = string.Empty;
        public string ResolvedById { get; set; } = string.Empty;
    }
}