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
    public async Task<ActionResult<ClientDto>> CreateClient([FromBody] ClientDto clientDto)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        
        var createdClient = await _clientService.CreateClient(clientDto, httpContext);
        
        return Created("", createdClient);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateClient([FromBody] ClientDto clientDto)
    {
        await _clientService.UpdateClient(clientDto);
        return Ok();
    }

    [HttpPost("{clientId}/update-session-duration")]
    public async Task<ActionResult> UpdateSessionDuration(int clientId)
    {
        await _clientService.UpdateSessionDuration(clientId);
        return Ok();
    }

    [HttpGet("{clientId}/download-app/{operatingSystem}")]
    public async Task<ActionResult> DownloadApp(int clientId, OperatingSystem operatingSystem)
    {
        var fileBytes = await _clientService.GetFile(clientId, operatingSystem);
        return File(fileBytes, "application/octet-stream", "lanstreamer");
    }
}