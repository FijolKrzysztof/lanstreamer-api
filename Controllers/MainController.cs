using lanstreamer_api.Models;
using lanstreamer_api.services;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.Controllers; 

[ApiController]
[Route("api/main")]
public class MainController : Controller
{
    public MainController(MainService mainService)
    {
        _mainService = mainService;
    }
    
    private readonly MainService _mainService;

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] User user)
    {
        return await _mainService.Login(user);
    }

    [HttpPost("authorize/{authorizationString}")]
    public async Task<ActionResult> Authorize(String authorizationString, [FromBody] User user)
    {
        return await _mainService.Authorize(authorizationString, user);
    }
}