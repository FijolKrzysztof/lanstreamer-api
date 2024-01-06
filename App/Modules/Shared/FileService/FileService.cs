using System.Net;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Configuration;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.services.FileService;

public class FileService : IFileService
{
    private readonly IConfigurationRepository _configurationRepository;
    
    public FileService(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }
    
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public FileStream ReadFileStream(string path)
    {
        return new FileStream(path, FileMode.Open, FileAccess.Read);
    }
    
    public async Task<string> GetDesktopAppPath(OperatingSystem os)
    {
        var dirPath = await _configurationRepository.GetByKey(ConfigurationKey.StoragePath);

        var filename = os switch
        {
            OperatingSystem.Windows => await _configurationRepository
                .GetByKey(ConfigurationKey.LanstreamerWindowsFilename),
            OperatingSystem.Linux => await _configurationRepository
                .GetByKey(ConfigurationKey.LanstreamerLinuxFilename),
            _ => throw new AppException(HttpStatusCode.UnsupportedMediaType, "Unsupported operating system")
        };
        
        return $"{dirPath}/{filename}";
    }
}