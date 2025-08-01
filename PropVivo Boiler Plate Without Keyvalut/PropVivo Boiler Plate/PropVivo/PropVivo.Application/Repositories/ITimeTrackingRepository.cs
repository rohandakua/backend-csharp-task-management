using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Repositories
{
    public interface ITimeTrackingRepository
    {
        Task<TimeTracking?> GetByIdAsync(string id);
        Task<List<TimeTracking>> GetAllAsync();
        Task<List<TimeTracking>> GetByUserIdAsync(string userId);
        Task<List<TimeTracking>> GetByUserIdAndDateAsync(string userId, DateTime date);
        Task<TimeTracking?> GetActiveByUserIdAsync(string userId);
        Task<TimeTracking?> GetByUserIdAndTaskIdAsync(string userId, string taskId);
        Task<TimeTracking> CreateAsync(TimeTracking timeTracking);
        Task<TimeTracking> UpdateAsync(TimeTracking timeTracking);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> HasActiveTrackingAsync(string userId);
        Task<decimal> GetTotalHoursByUserIdAndDateAsync(string userId, DateTime date);
        Task<decimal> GetTotalHoursByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
    }
} 