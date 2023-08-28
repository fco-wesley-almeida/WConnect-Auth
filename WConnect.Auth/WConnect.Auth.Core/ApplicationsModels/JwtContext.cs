using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.ApplicationsModels;

public class JwtContext
{
    public int UserId { get; }
    public string Login { get; }

    public JwtContext(User user)
    {
        UserId = user.Credential.Id;
        Login = user.Credential.Login.ToString();
    }
}