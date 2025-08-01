using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Features.Break.StartBreak
{
    public class StartBreakCommand : IRequest<BaseResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
        public BreakType Type { get; set; } = BreakType.Regular;
        public string? Reason { get; set; }
    }
} 