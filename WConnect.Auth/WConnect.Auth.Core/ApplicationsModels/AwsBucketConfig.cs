using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

namespace WConnect.Auth.Core.ApplicationsModels;

public class AwsBucketConfig
{
    private readonly string _bucketName;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly Guid _guid;
    public AwsBucketConfig(IConfiguration configuration)
    {
        _accessKey = configuration["Aws:AccessKey"]!;
        _secretKey = configuration["Aws:SecretKey"]!;
        _bucketName = configuration["Aws:BucketName"]!;
        _guid = Guid.NewGuid();
    }
    
    public BasicAWSCredentials BasicAwsCredentials => new (_accessKey, _secretKey);
    public AmazonS3Config AmazonS3Config =>
        new() {
            RegionEndpoint = Amazon.RegionEndpoint.USEast2
        };
    public Uri Uri => new($"https://{_bucketName}.s3.{SystemName}.{DnsSuffix}/{Path}");
    public TransferUtilityUploadRequest TransferUtilityUploadRequest(MemoryStream memoryStream) =>
        new()
        {
            InputStream = memoryStream,
            Key = Path,
            BucketName = _bucketName,
            CannedACL = S3CannedACL.PublicRead
        };
    
    private string SystemName => Amazon.RegionEndpoint.USEast2.SystemName;
    private string DnsSuffix => Amazon.RegionEndpoint.USEast2.PartitionDnsSuffix;
    private string Path => $"users_photos/{_guid}.jpg";
    
}