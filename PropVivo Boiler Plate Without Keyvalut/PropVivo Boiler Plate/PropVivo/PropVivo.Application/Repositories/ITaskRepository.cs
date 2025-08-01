using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Enums;
using Task = PropVivo.Domain.Entities.Task.Task;
using TaskStatus = PropVivo.Domain.Enums.TaskStatus;

namespace PropVivo.Application.Repositories
{
    public interface ITaskRepository
    {
        Task<Task?> GetByIdAsync(string id);
        Task<List<Task>> GetAllAsync();
        Task<List<Task>> GetByAssignedToIdAsync(string assignedToId);
        Task<List<Task>> GetByAssignedByIdAsync(string assignedById);
        Task<List<Task>> GetByStatusAsync(TaskStatus status);
        Task<List<Task>> GetActiveTasksByUserIdAsync(string userId);
        Task<Task> CreateAsync(Task task);
        Task<Task> UpdateAsync(Task task);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> HasActiveTaskAsync(string userId);
        Task<int> GetTaskCountByStatusAsync(string userId, TaskStatus status);
    }
} 