using lanstreamer_api.App.Attributes;
using lanstreamer_api.Models;
using lanstreamer_api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly UserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserController(UserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("login")]
    [Authorization]
    public async Task<ActionResult<LoginResponse>> Login(UserDto userDto)
    {
        var httpContext = _httpContextAccessor.HttpContext!;

        var loginResponse = await _userService.Login(userDto, httpContext);
        return Ok(loginResponse);
    }
}