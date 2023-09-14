using WConnect.Auth.Core.ApplicationsModels;

namespace WConnect.Auth.Core.Services;

public interface IStorageService
{
    Task UploadPhotoAsync(AwsBucketConfig config, byte[] file);
}