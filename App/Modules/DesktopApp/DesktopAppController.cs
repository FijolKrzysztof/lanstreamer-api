using System.Text;
using Microsoft.AspNetCore.Cors;
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

    [EnableCors("allowAll")]
    [HttpGet("access")]
    public async Task Access([FromQuery] float version, [FromQuery] string accessCode)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");

        await _desktopAppService.Access(version, accessCode, Response);
    }
}