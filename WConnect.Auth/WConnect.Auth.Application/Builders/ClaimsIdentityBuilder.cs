using System.Security.Claims;
using WConnect.Auth.Core.UseCases.SignIn;

namespace WConnect.Auth.Application.Builders;

public class ClaimsIdentityBuilder: IClaimsIdentityBuilder
{
    private readonly List<Claim> _claims;

    public ClaimsIdentityBuilder()
    {
        _claims = new();
    }

    public IClaimsIdentityBuilder AddClaim(string type, string value)
    {
        _claims.Add(new Claim(type, value));
        return this;
    }

    public ClaimsIdentity Build() => new(_claims);
}