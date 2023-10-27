using System.Net;
using Google.Apis.Auth;
using lanstreamer_api.App.Exceptions;
using Microsoft.Extensions.Primitives;

namespace lanstreamer_api.App.Middleware;

public class GoogleSignInMiddleware
{
    private readonly RequestDelegate _next;

    public GoogleSignInMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        string? idToken = null;

        if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
        {
            var headerValue = authHeader.First();
            if (headerValue is not null && headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                idToken = headerValue.Substring("Bearer ".Length).Trim();
            }
        }

        if (string.IsNullOrEmpty(idToken))
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Missing or invalid ID Token");
        }

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var identity = new System.Security.Claims.ClaimsIdentity(
                new []
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, payload.Subject),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, payload.Name)
                }
            );

            context.User = new System.Security.Claims.ClaimsPrincipal(identity);

            await _next(context);
        }
        catch (Exception e)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Invalid ID Token");
        }
    }
}

public static class GoogleSignInMiddlewareExtensions
{
    public static IApplicationBuilder UseGoogleSignInMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GoogleSignInMiddleware>();
    }
}