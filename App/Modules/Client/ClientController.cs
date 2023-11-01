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
        var createdClient = await _clientService.CreateClient(clientDto);
        return Created("", createdClient);
    }
    
    [HttpPut]
    public async Task<ActionResult<ClientDto>> UpdateClient([FromBody] ClientDto clientDto)
    {
        var client = await _clientService.UpdateClient(clientDto);
        return Ok(client);
    }
    
    [HttpGet("{clientId}/download/{operatingSystem}")]
    public async Task<ActionResult> Download(int clientId, OperatingSystem operatingSystem)
    {
        var fileBytes = await _clientService.GetFile(clientId, operatingSystem);
        return File(fileBytes, "application/octet-stream", "lanstreamer");
    }
}