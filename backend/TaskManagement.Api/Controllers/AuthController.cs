using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AppDbContext context,
        ILogger<AuthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == dto.Username);

        if (user is null)
        {
            _logger.LogWarning("Invalid login attempt for username {Username}", dto.Username);

            return Unauthorized(new
            {
                success = false,
                message = "Invalid username or password"
            });
        }

        var passwordHasher = new PasswordHasher<AppUser>();

        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Invalid password attempt for username {Username}", dto.Username);

            return Unauthorized(new
            {
                success = false,
                message = "Invalid username or password"
            });
        }

        _logger.LogInformation("User {Username} logged in successfully.", dto.Username);

        return Ok(new
        {
            success = true,
            message = "Login successful"
        });
    }
}