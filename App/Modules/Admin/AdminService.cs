using System.Net;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Modules.Admin;

public class AdminService
{
    public async Task UploadDesktopApp(OperatingSystem operatingSystem, IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            throw new AppException(HttpStatusCode.BadRequest, "File is empty");
        }

        if (Path.GetExtension(file.FileName).ToLower() != ".zip" || file.ContentType != "application/zip")
        {
            throw new AppException(HttpStatusCode.BadRequest, "Wrong file format. Accepting only ZIP files");
        }

        var filePath = ApplicationBuildPath.GetPath(operatingSystem);
        var stream = new FileStream(filePath, FileMode.Append);
        await file.CopyToAsync(stream);
    }
}