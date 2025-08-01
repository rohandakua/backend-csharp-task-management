using PropVivo.Domain.Entities.TaskQuery;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Repositories
{
    public interface ITaskQueryRepository
    {
        Task<TaskQuery?> GetByIdAsync(string id);
        Task<List<TaskQuery>> GetAllAsync();
        Task<List<TaskQuery>> GetByTaskIdAsync(string taskId);
        Task<List<TaskQuery>> GetByRaisedByIdAsync(string raisedById);
        Task<List<TaskQuery>> GetByAssignedToIdAsync(string assignedToId);
        Task<List<TaskQuery>> GetByStatusAsync(QueryStatus status);
        Task<List<TaskQuery>> GetByPriorityAsync(QueryPriority priority);
        Task<TaskQuery> CreateAsync(TaskQuery taskQuery);
        Task<TaskQuery> UpdateAsync(TaskQuery taskQuery);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<int> GetQueryCountByStatusAsync(string userId, QueryStatus status);
    }
} 