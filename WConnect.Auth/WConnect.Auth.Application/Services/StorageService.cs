using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using WConnect.Auth.Core.Services;
using WConnect.Auth.Application.Configuration;
using WConnect.Auth.Core.ApplicationsModels;

namespace WConnect.Auth.Application.Services;

public class StorageService: IStorageService
{
    public async Task UploadPhotoAsync(AwsBucketConfig config, byte[] file)
    {
        using var memoryStream = new MemoryStream(file);
        var uploadRequest = config.TransferUtilityUploadRequest(memoryStream);
        using var client = new AmazonS3Client(config.BasicAwsCredentials, config.AmazonS3Config);
        await new TransferUtility(client).UploadAsync(uploadRequest);
    }
}