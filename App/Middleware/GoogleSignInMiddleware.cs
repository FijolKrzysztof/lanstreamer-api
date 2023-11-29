using System.Net;
using System.Security.Claims;
using Google.Apis.Auth;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

namespace lanstreamer_api.App.Middleware;

public class GoogleSignInMiddleware
{
    private readonly RequestDelegate _next;

    public GoogleSignInMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context, ConfigurationRepository configurationRepository)
    {
        if (context.GetEndpoint()?.Metadata?.GetMetadata<IAuthorizeData>() == null)
        {
            await _next(context);
            return;
        }
        
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
            throw new AppException(HttpStatusCode.Unauthorized, "Missing or invalid google token");
        }

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            if (payload.Subject == null)
            {
                throw new AppException(HttpStatusCode.Unauthorized, "Cannot access google id");
            }

            if (payload.Email == null)
            {
                throw new AppException(HttpStatusCode.Unauthorized, "Cannot access email");
            }
            
            var identity = new ClaimsIdentity(
                new []
                {
                    new Claim(ClaimTypes.NameIdentifier, payload.Subject),
                    new Claim(ClaimTypes.Name, payload.Name),
                    new Claim(ClaimTypes.Email, payload.Email),
                }
            );

            var adminIdentifierObj = await configurationRepository.GetByKey("admin_identifier");

            if (adminIdentifierObj != null && payload.Subject == adminIdentifierObj.Value)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Admin));
            }

            context.User = new ClaimsPrincipal(identity);

            await _next(context);
        }
        catch (Exception e)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Invalid google token");
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