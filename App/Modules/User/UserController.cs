using lanstreamer_api.Models;
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
    
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UserDto userDto)
    {
        return await _userService.Update(userDto);
    }
}