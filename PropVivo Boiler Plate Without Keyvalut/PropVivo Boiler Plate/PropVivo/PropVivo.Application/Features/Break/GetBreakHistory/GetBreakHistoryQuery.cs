using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Break;

namespace PropVivo.Application.Features.Break.GetBreakHistory
{
    public class GetBreakHistoryQuery : IRequest<BaseResponse<List<BreakResponse>>>
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
} 