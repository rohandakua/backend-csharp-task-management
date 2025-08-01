using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Break;

namespace PropVivo.Application.Features.Break.GetTodayBreaks
{
    public class GetTodayBreaksQuery : IRequest<BaseResponse<List<BreakResponse>>>
    {
        public string UserId { get; set; } = string.Empty;
    }
} 