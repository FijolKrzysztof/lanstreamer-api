using lanstreamer_api.App.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules.Admin;

[ApiController]
[Route("api/admin")]
public class AdminController
{
    [Authorize(Roles = Roles.Admin)]
    [HttpPost("upload-desktop-app")]
    public ActionResult UploadDesktopApp()
    {
        // TODO
    }
}