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

    [HttpPost("login")]
    [Authorize]
    public async Task<ActionResult> Login(UserDto userDto)
    {
        var httpContext = HttpContext;

        await _userService.Login(userDto, httpContext);
        return Ok("");
    }
}