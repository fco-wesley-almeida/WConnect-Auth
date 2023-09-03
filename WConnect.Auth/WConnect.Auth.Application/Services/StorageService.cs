using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using WConnect.Auth.Core.Services;
using WConnect.Auth.Application.Configuration;

namespace WConnect.Auth.Application.Services;

public class StorageService: IStorageService
{
    private readonly IConfiguration _configuration;
    
    public StorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<Uri> UploadPhotoAsync(byte[] file)
    {
        using var memoryStream = new MemoryStream(file);
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.USEast2
        };
        var systemName = Amazon.RegionEndpoint.USEast2.SystemName;
        var dnsSuffix = Amazon.RegionEndpoint.USEast2.PartitionDnsSuffix;
        var bucketName = _configuration.GetString("Aws:BucketName");
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = memoryStream,
            Key = $"users_photos/{Guid.NewGuid()}.jpg",
            BucketName = bucketName,
            CannedACL = S3CannedACL.PublicRead
        };
        var credentials = new BasicAWSCredentials(
            _configuration.GetString("Aws:AccessKey"), 
            _configuration.GetString("Aws:SecretKey")
        );
        using var client = new AmazonS3Client(credentials, config);
        var transferUtility = new TransferUtility(client);
        await transferUtility.UploadAsync(uploadRequest);
        return new Uri($"https://{bucketName}.s3.{systemName}.{dnsSuffix}/{uploadRequest.Key}");
    }
}