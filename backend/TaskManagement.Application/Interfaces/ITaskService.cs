using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllAsync();
    Task<TaskDto?> GetByIdAsync(int id);
    Task<TaskDto> CreateAsync(TaskCreateDto dto);
    Task<bool> UpdateAsync(int id, TaskUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> MarkAsCompletedAsync(int id);
}