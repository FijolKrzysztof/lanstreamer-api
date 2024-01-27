using System.Net;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.services.FileService;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Modules.Admin;

public class AdminService
{
    private readonly IFileService _fileService;
    
    public AdminService(IFileService fileService)
    {
        _fileService = fileService;
    }
    
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
            var filePath = await _fileService.GetDesktopAppPath(operatingSystem);
            var directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath) && directoryPath is not null)
            {
                Directory.CreateDirectory(directoryPath);
            }

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch (Exception e)
        {
            throw new AppException(HttpStatusCode.InternalServerError, "Error occurred while saving or zipping file", e);
        }
    }
}