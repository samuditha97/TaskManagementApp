using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var username = _configuration["AuthSettings:Username"];
        var password = _configuration["AuthSettings:Password"];

        if (dto.Username == username && dto.Password == password)
        {
            _logger.LogInformation("User logged in successfully.");

            return Ok(new
            {
                success = true,
                message = "Login successful"
            });
        }

        _logger.LogWarning("Invalid login attempt for username {Username}", dto.Username);

        return Unauthorized(new
        {
            success = false,
            message = "Invalid username or password"
        });
    }
}