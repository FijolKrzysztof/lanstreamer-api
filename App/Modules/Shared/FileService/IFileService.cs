using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.services.FileService;

public interface IFileService
{
    bool Exists(string path);
    FileStream ReadFileStream(string path);
    Task<string> GetDesktopAppPath(OperatingSystem os);
}