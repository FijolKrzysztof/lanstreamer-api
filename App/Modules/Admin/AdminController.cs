using lanstreamer_api.App.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Modules.Admin;

[ApiController]
[Route("api/admin")]
public class AdminController
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    [Authorize(Roles = Roles.Admin)]
    [HttpPost("upload-desktop-app")]
    public ActionResult UploadDesktopApp([FromQuery] OperatingSystem operatingSystem, IFormFile file)
    {
        return _adminService.UploadDesktopApp(operatingSystem, file);
    }
}