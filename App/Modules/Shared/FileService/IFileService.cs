namespace lanstreamer_api.services.FileService;

public interface IFileService
{
    bool Exists(string path);
    FileStream ReadFileStream(string path);
}