using Ambev.DeveloperEvaluation.WebApi.Common;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware;

public sealed class UnauthorizedResponseMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized &&
                !context.Request.Path.StartsWithSegments("/api/auth"))
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Usuário não está autenticado. Faça login pelo endpoint api/auth para obter um token."
                };

                await JsonSerializer.SerializeAsync(context.Response.Body, response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            responseBody.Position = 0;
            await responseBody.CopyToAsync(originalBody);
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }
}
