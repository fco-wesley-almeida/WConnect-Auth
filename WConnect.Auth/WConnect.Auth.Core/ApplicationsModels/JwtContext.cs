using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.ApplicationsModels;

public class JwtContext
{
    public int UserId { get; }

    public JwtContext(User user)
    {
        UserId = user.Credential.Id ?? throw new ArgumentNullException(nameof(user.Credential.Id));
    }
}