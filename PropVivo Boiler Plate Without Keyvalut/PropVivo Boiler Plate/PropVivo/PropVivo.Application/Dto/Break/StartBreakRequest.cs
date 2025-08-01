using System.ComponentModel.DataAnnotations;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.Break
{
    public class StartBreakRequest
    {
        [Required]
        public BreakType Type { get; set; } = BreakType.Regular;
        
        public string? Reason { get; set; }
    }
} 