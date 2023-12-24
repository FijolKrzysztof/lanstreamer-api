using System.Net;
using System.Security.Claims;
using Google.Apis.Auth;
using lanstreamer_api.App.Attributes;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.App.Modules.Shared.GoogleAuthenticationService;
using lanstreamer_api.Data.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace lanstreamer_api.App.Middleware;

public class GoogleSignInMiddleware
{
    private readonly RequestDelegate _next;

    public GoogleSignInMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ConfigurationRepository configurationRepository,
        IGoogleAuthenticationService googleAuthenticationService)
    {
        var authorizationAttribute = context.GetEndpoint()?.Metadata?.GetMetadata<AuthorizationAttribute>();

        if (authorizationAttribute == null)
        {
            await _next(context);
            return;
        }

        string? idToken = null;

        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var headerValue = authHeader.First();
            if (headerValue is not null && headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                idToken = headerValue.Substring("Bearer ".Length).Trim();
            }
        }

        if (string.IsNullOrEmpty(idToken))
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Missing google token");
        }

        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await googleAuthenticationService.VerifyGoogleToken(idToken);
        }
        catch (Exception e)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Invalid google token");
        }
        
        if (payload.Subject == null)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Cannot access google id");
        }

        if (payload.Email == null)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Cannot access email");
        }
        
        if (payload.Name == null)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Cannot access name");
        }

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, payload.Subject),
                new Claim(ClaimTypes.Name, payload.Name),
                new Claim(ClaimTypes.Email, payload.Email),
            }
        );
        
        var adminIdentifierObj = await configurationRepository.GetByKey(ConfigurationKey.AdminIdentifier);

        if (adminIdentifierObj != null && payload.Subject == adminIdentifierObj.Value)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, Role.Admin.ToString()));
        }

        identity.AddClaim(new Claim(ClaimTypes.Role, Role.User.ToString()));

        context.User = new ClaimsPrincipal(identity);

        await authorizationAttribute.CheckRole(context);
        await _next(context);
    }
}