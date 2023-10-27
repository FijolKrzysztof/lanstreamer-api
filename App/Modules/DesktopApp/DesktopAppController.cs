using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules.Access;

[ApiController]
[Route("api/desktop-app")]
public class DesktopAppController : Controller
{
    private readonly DesktopAppService _desktopAppService;

    public DesktopAppController(DesktopAppService desktopAppService)
    {
        _desktopAppService = desktopAppService;
    }
    
    [HttpGet("access")]
    public async Task<bool> Access([FromQuery] float version, [FromQuery] string accessCode)
    {
        return await _desktopAppService.Access(version, accessCode);
    }
}