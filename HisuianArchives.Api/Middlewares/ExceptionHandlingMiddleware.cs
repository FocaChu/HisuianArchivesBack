using HisuianArchives.Application.Exceptions; 
using System.Net;
using System.Text.Json;

namespace HisuianArchives.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception has occurred: {Message}", exception.Message);

        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            case BusinessException businessException:
                statusCode = HttpStatusCode.BadRequest;
                message = businessException.Message;
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected internal server error has occurred. Please try again later.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new { title = statusCode.ToString(), status = (int)statusCode, detail = message };
        var jsonResponse = JsonSerializer.Serialize(errorResponse);

        await context.Response.WriteAsync(jsonResponse);
    }
}