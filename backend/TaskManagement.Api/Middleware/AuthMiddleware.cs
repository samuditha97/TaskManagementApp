using System.Net;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Api.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;

    public AuthMiddleware(
        RequestDelegate next,
        ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        if (!context.Request.Path.StartsWithSegments("/api/tasks"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            await Unauthorized(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();

        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            await Unauthorized(context);
            return;
        }

        try
        {
            var encodedCredentials = authHeader["Basic ".Length..].Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            var parts = decodedCredentials.Split(':', 2);

            if (parts.Length != 2)
            {
                await Unauthorized(context);
                return;
            }

            var username = parts[0];
            var password = parts[1];

            var user = await dbContext.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user is null)
            {
                _logger.LogWarning("Unauthorized API access attempt for username {Username}", username);
                await Unauthorized(context);
                return;
            }

            var passwordHasher = new PasswordHasher<AppUser>();

            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password);

            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Invalid API password attempt for username {Username}", username);
                await Unauthorized(context);
                return;
            }

            await _next(context);
        }
        catch
        {
            await Unauthorized(context);
        }
    }

    private static async Task Unauthorized(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync("""
        {
          "message": "Unauthorized"
        }
        """);
    }
}