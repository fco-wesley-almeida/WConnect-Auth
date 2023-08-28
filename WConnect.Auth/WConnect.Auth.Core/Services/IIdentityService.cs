using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.Services;

public interface IIdentityService
{
    string Encrypt(int userId);
    int Decrypt(string accessToken);
}