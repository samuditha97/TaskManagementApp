using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);

        if (task is null)
            return NotFound("Task not found.");

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskCreateDto dto)
    {
        var task = await _taskService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
    {
        var updated = await _taskService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound("Task not found.");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteAsync(id);

        if (!deleted)
            return NotFound("Task not found.");

        return NoContent();
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> MarkAsCompleted(int id)
    {
        var updated = await _taskService.MarkAsCompletedAsync(id);

        if (!updated)
            return NotFound("Task not found.");

        return NoContent();
    }
}