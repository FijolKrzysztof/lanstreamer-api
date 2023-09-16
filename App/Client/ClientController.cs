using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;

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
}