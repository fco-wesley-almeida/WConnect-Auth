using System.Security.Claims;

namespace WConnect.Auth.Core.UseCases.SignIn;

public interface IClaimsIdentityBuilder
{
    IClaimsIdentityBuilder AddClaim(string type, string value);
    ClaimsIdentity Build();
}