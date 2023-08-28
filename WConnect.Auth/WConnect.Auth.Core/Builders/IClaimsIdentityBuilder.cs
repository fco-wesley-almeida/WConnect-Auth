using System.Security.Claims;

namespace WConnect.Auth.Core.Builders;

public interface IClaimsIdentityBuilder
{
    IClaimsIdentityBuilder AddClaim(string type, string value);
    ClaimsIdentity Build();
}