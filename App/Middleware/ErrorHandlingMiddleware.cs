using lanstreamer_api.App.Data.Dto.Responses;
using lanstreamer_api.App.Exceptions;
using Newtonsoft.Json;

namespace lanstreamer_api.App.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            if (ex is AppException appException)
            {
                var exception = appException.Exception;
                if (exception != null)
                {
                    await Console.Error.WriteAsync(exception.ToString());
                }
                
                await Console.Error.WriteAsync(appException.Message);

                context.Response.StatusCode = (int)appException.HttpStatusCode;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = appException.Message,
                };
                
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);

                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal server error",
                };
            
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);

                await context.Response.WriteAsync(jsonResponse);
            }
            
            _logger.LogError(ex, "Error occurred");
        }
    }
}
