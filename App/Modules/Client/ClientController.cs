using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Client;

[ApiController]
[Route("api/client")]
public class ClientController : Controller
{
    private readonly ClientService _clientService;

    public ClientController(ClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateClient([FromBody] ClientDto clientDto)
    {
        var xForwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        var acceptLanguage = HttpContext.Request.Headers["Accept-Language"].ToString();
        
        var createdClient = await _clientService.CreateClient(clientDto, xForwardedFor, userAgent, acceptLanguage);
        return Created("", createdClient);
    }
    
    // TODO: zaimplementować websocket przy przesyłaniu informacji że klient dalej jest na stronie żeby zapisywać czas (time on site)
    
    [HttpPut]
    public async Task<ActionResult<ClientDto>> UpdateClient([FromBody] ClientDto clientDto)
    {
        var client = await _clientService.UpdateClient(clientDto);
        return Ok(client);
    }
    
    [HttpGet("{clientId}/download-app/{operatingSystem}")]
    public async Task<ActionResult> DownloadApp(int clientId, OperatingSystem operatingSystem)
    {
        var fileBytes = await _clientService.GetFile(clientId, operatingSystem);
        return File(fileBytes, "application/octet-stream", "lanstreamer");
    }
}