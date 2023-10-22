using Amazon.Runtime.Internal;
using lanstreamer_api.App.Exceptions;
using Newtonsoft.Json;

namespace lanstreamer_api.App.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
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
                context.Response.StatusCode = (int)appException.HttpStatusCode;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse() { Message = appException.Message };
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}