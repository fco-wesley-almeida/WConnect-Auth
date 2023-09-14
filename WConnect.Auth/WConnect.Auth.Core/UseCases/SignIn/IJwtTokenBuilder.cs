using Microsoft.IdentityModel.Tokens;

namespace WConnect.Auth.Core.UseCases.SignIn;

public interface IJwtTokenBuilder
{
    IJwtTokenBuilder WithSecurityTokenDescriptor(SecurityTokenDescriptor securityTokenDescriptor);
    JwtToken Build();
}