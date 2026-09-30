using System.Text.Json;
using HealthFlow.Service.Exceptions;

namespace HealthFlow.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessRuleException exception)
        {
            _logger.LogWarning(
                exception,
                "Regra de negócio violada.");

            await WriteErrorResponseAsync(
                context,
                StatusCodes.Status400BadRequest,
                exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Erro não tratado durante a requisição.");

            await WriteErrorResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro interno. Tente novamente mais tarde.");
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            statusCode,
            message,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path.ToString()
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
