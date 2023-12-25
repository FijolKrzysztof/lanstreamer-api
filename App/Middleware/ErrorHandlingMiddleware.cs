using lanstreamer_api.App.Data.Dto.Responses;
using lanstreamer_api.App.Exceptions;
using Newtonsoft.Json;

namespace lanstreamer_api.App.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
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
            
            logger.LogError(ex, "Error occurred");
        }
    }
}
