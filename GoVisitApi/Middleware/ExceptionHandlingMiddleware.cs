using GoVisit.Core.Models;
using System.Net;
using System.Text.Json;

namespace GoVisitApi.Middleware;

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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var (statusCode, response) = exception switch
        {
            ArgumentException argEx => ((int)HttpStatusCode.BadRequest, new ApiResponse
            {
                Success = false,
                Message = $"Error - GoVisit: {argEx.Message}",
                ErrorCode = "VALIDATION_ERROR",
                StatusCode = (int)HttpStatusCode.BadRequest,
                Timestamp = DateTime.UtcNow
            }),
            KeyNotFoundException => ((int)HttpStatusCode.NotFound, 
                ApiResponse.ErrorResult("Error - GoVisit: המשאב המבוקש לא נמצא", "NOT_FOUND", (int)HttpStatusCode.NotFound)),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, 
                ApiResponse.ErrorResult("Error - GoVisit: אין הרשאה לבצע פעולה זו", "UNAUTHORIZED", (int)HttpStatusCode.Unauthorized)),
            _ => ((int)HttpStatusCode.InternalServerError, 
                ApiResponse.ErrorResult("Error - GoVisit: אירעה שגיאה במערכת. אנא נסה שוב מאוחר יותר", "INTERNAL_ERROR", (int)HttpStatusCode.InternalServerError))
        };

        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}