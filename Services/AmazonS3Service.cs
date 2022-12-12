using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace lanstreamer_api.services;

public class AmazonS3Service
{
    private readonly AmazonS3Client _s3Client = new ("AKIAXZOBUS5A3FFUHWRS",
        "VzPUkL/gF44JiKeKdTETuwl7KCmQ1plzv1toWjjN", RegionEndpoint.EUWest2);
    private readonly string _bucket = "lanstreamer";

    public static string ConvertStreamToString(Stream stream)
    {
        int oneByte;
        string result = "";
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
            await _s3Client.GetObjectAsync(_bucket, s3Path).Result.ResponseStream.CopyToAsync(stream);
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
        return (await _s3Client.ListObjectsAsync(_bucket, s3Path)).S3Objects.Count > 0;
    }

    public async Task<List<S3Object>> GetObjectList(string s3Path)
    {
        return (await _s3Client.ListObjectsAsync(_bucket, s3Path)).S3Objects;
    }
}