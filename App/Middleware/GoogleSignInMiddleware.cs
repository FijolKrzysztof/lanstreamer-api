using System.Net;
using System.Security.Claims;
using Google.Apis.Auth;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Configuration;
using Microsoft.Extensions.Primitives;

namespace lanstreamer_api.App.Middleware;

public class GoogleSignInMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConfigurationRepository _configurationRepository;

    public GoogleSignInMiddleware(RequestDelegate next, ConfigurationRepository configurationRepository)
    {
        _next = next;
        _configurationRepository = configurationRepository;
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
            var identity = new ClaimsIdentity(
                new []
                {
                    new Claim(ClaimTypes.NameIdentifier, payload.Subject),
                    new Claim(ClaimTypes.Name, payload.Name)
                }
            );

            var adminIdentifierObj = await _configurationRepository.GetByKey("admin_identifier");

            if (adminIdentifierObj != null && payload.Subject == adminIdentifierObj.Value)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, Roles.Admin));
            }

            context.User = new ClaimsPrincipal(identity);

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