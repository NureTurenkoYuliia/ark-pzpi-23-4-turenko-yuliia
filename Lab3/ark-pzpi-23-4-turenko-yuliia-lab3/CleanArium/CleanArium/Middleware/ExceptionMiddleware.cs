namespace CleanArium.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;

                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message
            });
        }
    }
}
