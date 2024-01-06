using System.Net;
using lanstreamer_api.App.Attributes;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Modules.Admin;

[ApiController]
[Route("api/admin")]
public class AdminController : Controller
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    [Authorization(Role.Admin)]
    [HttpPost("upload-desktop-app")]
    [RequestSizeLimit(1000000000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 10000000000)] 
    public async Task<ActionResult> UploadDesktopApp([FromQuery][BindRequired] OperatingSystem operatingSystem, [FromForm] IFormFile file)
    {
        // TODO: dodać testy z dużymi plikami
        await _adminService.UploadDesktopApp(operatingSystem, file);
        return Ok();
    }
}