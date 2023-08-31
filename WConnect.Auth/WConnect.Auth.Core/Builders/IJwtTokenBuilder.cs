using Microsoft.IdentityModel.Tokens;
using WConnect.Auth.Core.ApplicationsModels;

namespace WConnect.Auth.Core.Builders;

public interface IJwtTokenBuilder
{
    IJwtTokenBuilder WithSecurityTokenDescriptor(SecurityTokenDescriptor securityTokenDescriptor);
    JwtToken Build();
}