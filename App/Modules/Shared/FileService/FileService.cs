namespace lanstreamer_api.services.FileService;

public class FileService : IFileService
{
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public FileStream ReadFileStream(string path)
    {
        return new FileStream(path, FileMode.Open, FileAccess.Read);
    }
}