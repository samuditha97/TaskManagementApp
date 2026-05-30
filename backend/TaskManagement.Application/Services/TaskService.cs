using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> GetAllAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();

        return tasks.Select(MapToDto);
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        return task is null ? null : MapToDto(task);
    }

    public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Task title is required.");

        var task = new TaskItem
        {
            Title = dto.Title.Trim(),
            Description = dto.Description,
            DueDate = dto.DueDate,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var createdTask = await _taskRepository.AddAsync(task);

        return MapToDto(createdTask);
    }

    public async Task<bool> UpdateAsync(int id, TaskUpdateDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Task title is required.");

        task.Title = dto.Title.Trim();
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;
        task.IsCompleted = dto.IsCompleted;

        await _taskRepository.UpdateAsync(task);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            return false;

        await _taskRepository.DeleteAsync(task);

        return true;
    }

    public async Task<bool> MarkAsCompletedAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            return false;

        task.IsCompleted = true;

        await _taskRepository.UpdateAsync(task);

        return true;
    }

    private static TaskDto MapToDto(TaskItem task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            DueDate = task.DueDate
        };
    }
}