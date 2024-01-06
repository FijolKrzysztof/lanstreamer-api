using lanstreamer_api.App.Data.Dto;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Client;

[ApiController]
[Route("api/client")]
public class ClientController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClientService _clientService;

    public ClientController(ClientService clientService, IHttpContextAccessor httpContextAccessor)
    {
        _clientService = clientService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<ActionResult<CreatedObjResponse>> CreateClient([FromBody] ClientDto clientDto)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        // TODO: naprawić pobieranie IP
        var createdClientResponse = await _clientService.CreateClient(clientDto, httpContext);
        
        return Created("", createdClientResponse);
    }
    
    [HttpPost("{clientId}/add-feedback")]
    public async Task<ActionResult> AddFeedbacks(int clientId, [FromBody] string feedback)
    {
        await _clientService.AddFeedbacks(clientId, feedback);
        return Ok();
    }

    [HttpPost("{clientId}/update-session-duration")]
    public async Task<ActionResult> UpdateSessionDuration(int clientId)
    {
        await _clientService.UpdateSessionDuration(clientId);
        return Ok();
    }

    [HttpGet("{clientId}/download-app/{operatingSystem}")]
    public async Task<FileStreamResult> DownloadApp(int clientId, OperatingSystem operatingSystem)
    {
        Response.Headers.Add("Content-Disposition", "attachment; filename=lanstreamer.zip");
        
        var stream = await _clientService.GetFileStream(clientId, operatingSystem);
        
        return new FileStreamResult(stream, "application/octet-stream");
    }
    
    [HttpHead("{clientId}/download-app/{operatingSystem}")]
    public async Task<IActionResult> CheckAppExistence(int clientId, OperatingSystem operatingSystem)
    {
        // TODO: testy
        
        var stream = await _clientService.GetFileStream(clientId, operatingSystem);

        if (stream != null)
        {
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}