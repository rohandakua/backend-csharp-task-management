using PropVivo.Domain.Entities.Break;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Repositories
{
    public interface IBreakRepository
    {
        Task<Break?> GetByIdAsync(string id);
        Task<List<Break>> GetAllAsync();
        Task<List<Break>> GetByUserIdAsync(string userId);
        Task<List<Break>> GetByTimeTrackingIdAsync(string timeTrackingId);
        Task<List<Break>> GetByUserIdAndDateAsync(string userId, DateTime date);
        Task<Break?> GetActiveByUserIdAsync(string userId);
        Task<Break> CreateAsync(Break breakItem);
        Task<Break> UpdateAsync(Break breakItem);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> HasActiveBreakAsync(string userId);
        Task<decimal> GetTotalBreakHoursByUserIdAndDateAsync(string userId, DateTime date);
    }
} 