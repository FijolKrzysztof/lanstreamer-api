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

    [HttpPost] // tutaj raczej trzeba będzie dodać middleware które będzie sprawdzało dane logowania czyli idToken i w put pewnie to samo i raczej przekazywanie przez Bearer token tego google id token
    public async Task<ActionResult<UserDto>> Create([FromQuery] string idToken, [FromBody] UserDto userDto)
    {
        var user = await _userService.Create(userDto, idToken);
        return Created("", user);
    }
    
    [HttpPut] // zastanowić się jak ma wyglądać autoryzacja, bo chyba trzeba będzie zrobić jakieś middleware zamiast w każdym endpoincie poisać obsługę sprawdzania idToken
    public async Task<ActionResult> Update([FromQuery] string idToken, [FromBody] UserDto userDto)
    {
        return await _userService.Update(userDto);
    }
}