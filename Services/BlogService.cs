using Amazon.S3;
using Amazon.S3.Model;

namespace lanstreamer_api.services;

public class BlogService
{
    public BlogService(AmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }
    
    private readonly AmazonS3Service _amazonS3Service;
    public async Task<string> GetBlogContent(string title)
    {
        return await new StreamReader(await _amazonS3Service.GetObjectAsStream($"blogs/{title}.html")).ReadToEndAsync();
    }

    public async Task<Stream> GetBlogImage(string title)
    {
        string[] imageFormats = { "jpg", "png" };
        foreach (var imageFormat in imageFormats)
        {
            if (await _amazonS3Service.IsObjectInBucket($"blogs/{title}.{imageFormat}"))
            {
                return await _amazonS3Service.GetObjectAsStream($"blogs/{title}.{imageFormat}");
            };
        }
        throw new FileNotFoundException();
    }

    public async Task<List<string>> GetBlogs()
    {
        return (await _amazonS3Service.GetObjectList("blogs/"))
            .Select(obj => obj.Key!)
            .Where(name => name.Contains(".html"))
            .Select(name => name.Substring("blogs/".Length, name.Length - ".html".Length - "blogs/".Length))
            .ToList();
    }
}
