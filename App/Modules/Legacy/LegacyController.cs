using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules.Legacy;

[ApiController]
public class LegacyController : Controller
{
    private readonly LegacyService _legacyService;
    
    public LegacyController(LegacyService legacyService)
    {
        _legacyService = legacyService;
    }
    
    [HttpGet("api/main/app/access/{authorizationString}/{version}")]
    public async Task<string> AppAccess(string authorizationString, string version)
    {
        return await _legacyService.AppAccess(authorizationString, version);
    }
}