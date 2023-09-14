namespace WConnect.Auth.Core.UseCases.SignUp;

public interface IStorageService
{
    Task UploadPhotoAsync(AwsBucketConfig config, byte[] file);
}