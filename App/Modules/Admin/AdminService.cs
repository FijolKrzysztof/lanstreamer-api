using System.IO.Compression;
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

        if (Path.GetExtension(file.FileName) != ".zip")
        {
            throw new AppException(HttpStatusCode.BadRequest, "Wrong file format. Accepting only ZIP files");
        }

        try
        {
            var filePath = ApplicationBuildPath.GetPath(operatingSystem);
            var directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        catch (Exception e)
        {
            throw new AppException(HttpStatusCode.InternalServerError, "Error occurred while saving or zipping file");
        }
    }
}