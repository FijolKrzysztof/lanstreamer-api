using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace lanstreamer_api.services;

public class AmazonS3Service
{
    private readonly AmazonS3Client _s3Client;
    private const string BucketName = "lanstreamer";

    public AmazonS3Service(IConfiguration configuration)
    {
        _s3Client = new AmazonS3Client(configuration.GetConnectionString("aws_access_key_id"),
            configuration.GetConnectionString("aws_secret_access_key"), RegionEndpoint.EUWest2);
    }
    
    public static string ConvertStreamToString(Stream stream)
    {
        int oneByte;
        var result = "";
        while ((oneByte = stream.ReadByte()) != -1)
        {
            result += (char)oneByte;
        }
        return result;
    }

    public async Task<Stream> GetObjectAsStream(string s3Path)
    {
        try
        {
            var stream = new MemoryStream();
            await _s3Client.GetObjectAsync(BucketName, s3Path).Result.ResponseStream.CopyToAsync(stream);
            stream.Position = 0;
            return stream;
        }
        catch (Exception)
        {
            throw new FileNotFoundException();
        }
    }

    public async Task<bool> IsObjectInBucket(string s3Path)
    {
        return (await _s3Client.ListObjectsAsync(BucketName, s3Path)).S3Objects.Count > 0;
    }

    public async Task<List<S3Object>> GetObjectList(string s3Path)
    {
        return (await _s3Client.ListObjectsAsync(BucketName, s3Path)).S3Objects;
    }
}