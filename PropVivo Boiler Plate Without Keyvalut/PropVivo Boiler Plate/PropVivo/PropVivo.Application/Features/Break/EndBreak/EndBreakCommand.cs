using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Features.Break.EndBreak
{
    public class EndBreakCommand : IRequest<BaseResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
    }
} 