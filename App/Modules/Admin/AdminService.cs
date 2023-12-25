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

        try
        {
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            new ZipArchive(memoryStream);
        }
        catch (Exception e)
        {
            throw new AppException(HttpStatusCode.BadRequest, "Wrong file format. Accepting only ZIP files");
        }
        
        var filePath = ApplicationBuildPath.GetPath(operatingSystem);
        var directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            var stream = new FileStream(filePath, FileMode.Append);
            await file.CopyToAsync(stream);
        }
        catch (Exception e)
        {
            throw new AppException(HttpStatusCode.InternalServerError, "Error occurred while saving file");
        }
    }
}