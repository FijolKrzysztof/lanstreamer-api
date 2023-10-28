using System.Text;
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
    public async Task Access([FromQuery] float version, [FromQuery] string accessCode)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        
        var reader = await _desktopAppService.Access(version, accessCode);

        await foreach (var item in reader.ReadAllAsync())
        {
            await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(item.ToString()));
            await Response.Body.FlushAsync();
        }
    }
}