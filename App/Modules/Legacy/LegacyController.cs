using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules.Legacy;

[ApiController]
public class LegacyController : Controller
{
    [HttpGet("api/main/app/access/{authorizationString}/{version}")]
    public async Task<string> AppAccess(string authorizationString, string version)
    {
        return await _mainService.AppAccess(authorizationString, version);
    }
}