using lanstreamer_api.App.Data.Models;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.services;

public interface IHttpRequestInfoService
{
    List<string> GetRoles(HttpContext httpContext);
    string? GetEmail(HttpContext httpContext);
    string? GetIdentity(HttpContext httpContext);
    string? GetIpAddress(HttpContext httpContext);
    OperatingSystem GetOs(HttpContext httpContext);
    string? GetDefaultLanguage(HttpContext httpContext);
    Task<IpLocation> GetIpLocation(string ip);
}