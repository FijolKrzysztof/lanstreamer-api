using lanstreamer_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UserDto>> Create([FromBody] UserDto userDto)
    {
        var user = await _userService.Create(userDto);
        return Created("", user);
    }
    
    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UserDto userDto)
    {
        var user = await _userService.Update(userDto);
        return Ok(user);
    }
    
    // TODO: SSE do app access
    // TODO: Scheduler usuwający access które są stare
}