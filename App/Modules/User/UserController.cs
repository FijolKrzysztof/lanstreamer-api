using lanstreamer_api.Models;
using lanstreamer_api.Models.Responses;
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
    public async Task<ActionResult<LoginResponseDto>> Login(UserDto userDto)
    {
        var httpContext = HttpContext;

        var loginResponse = await _userService.Login(userDto, httpContext);
        return Ok(loginResponse);
    }
}