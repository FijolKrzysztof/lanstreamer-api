namespace lanstreamer_api.services;

public class DemoService
{
    public DemoService(AmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    private readonly AmazonS3Service _amazonS3Service;
    
    public async Task<Stream> Preview(string filename, int timestampId)
    {
        try
        {
            var name = filename.Remove(filename.Length - 4);
            var number = String.Format("{0:D4}", timestampId);
            var extension = filename.Remove(0, filename.Length - 3);
            return await _amazonS3Service.GetObjectAsStream($"previews/{name}/scr-{number}.{extension}");
        }
        catch (Exception)
        {
            return await _amazonS3Service.GetObjectAsStream("previews/NoPreview.png");
        }
    }

    public async Task<Stream> Video(string filename)
    {
        return await _amazonS3Service.GetObjectAsStream($"videos/{filename}");
    }

    public async Task<Stream> Poster(string filename)
    {
        return await _amazonS3Service.GetObjectAsStream($"posters/{filename}");
    }
}