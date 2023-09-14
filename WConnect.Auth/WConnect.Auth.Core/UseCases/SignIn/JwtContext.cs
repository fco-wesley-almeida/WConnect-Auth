using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.UseCases.SignIn;

public class JwtContext
{
    public int UserId { get; }

    public JwtContext(User user)
    {
        UserId = user.Credential.Id ?? throw new ArgumentNullException(nameof(user.Credential.Id));
    }
}