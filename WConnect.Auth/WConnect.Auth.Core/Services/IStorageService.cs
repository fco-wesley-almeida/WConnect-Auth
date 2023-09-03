namespace WConnect.Auth.Core.Services;

public interface IStorageService
{
    Task<Uri> UploadPhotoAsync(byte[] file);
}