using System.Security.Claims;
using lanstreamer_api.App.Data.Models;
using Newtonsoft.Json;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.services;

public class HttpRequestInfoService
{
    private readonly ILogger<HttpRequestInfoService> _logger;

    public HttpRequestInfoService(ILogger<HttpRequestInfoService> logger)
    {
        _logger = logger;
    }
    
    public List<string> GetRoles(HttpContext httpContext)
    {
        return httpContext.User.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToList();
    }

    public string? GetEmail(HttpContext httpContext)
    {
        return httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
    }

    public string? GetIdentity(HttpContext httpContext)
    {
        return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string? GetIpAddress(HttpContext httpContext)
    {
        var xForwardedForHeader = httpContext.Request.Headers["X-Forwarded-For"].ToString();
        
        if (string.IsNullOrEmpty(xForwardedForHeader))
        {
            return null;
        }

        var ipAddresses = xForwardedForHeader.Split(',');

        return ipAddresses[0].Trim();
    }

    public OperatingSystem GetOs(HttpContext httpContext)
    {
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        
        if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.Windows;
        }

        if (userAgent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.MacOS;
        }

        if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.Linux;
        }

        if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.Android;
        }

        if (userAgent.Contains("iOS", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.IOS;
        }

        return OperatingSystem.Other;
    }

    public string? GetDefaultLanguage(HttpContext httpContext)
    {
        var acceptLanguage = httpContext.Request.Headers["Accept-Language"].ToString();
        
        if (string.IsNullOrEmpty(acceptLanguage))
        {
            return null;
        }

        var languages = acceptLanguage.Split(',');
        var firstLanguage = languages[0]?.Trim();
        return firstLanguage?.Split('-')[0];
    }

    public async Task<IpLocation> GetIpLocation(string ip)
    {
        var ipLocation = new IpLocation()
        {
            Ip = ip,
        };
        try
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"http://ipinfo.io/{ip}");

            ipLocation = JsonConvert.DeserializeObject<IpLocation>(response) ?? ipLocation;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Error occurred while fetching IP location for address {IpAddress}. Details: {ErrorMessage}", ip,
                e.Message);
        }

        return ipLocation;
    }
}