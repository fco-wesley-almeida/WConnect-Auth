using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using WConnect.Auth.Core.ApplicationsModels;
using WConnect.Auth.Core.Builders;

namespace WConnect.Auth.Application.Builders;

public class JwtTokenBuilder: IJwtTokenBuilder
{
    private SecurityTokenDescriptor? _securityTokenDescriptor;

    public IJwtTokenBuilder WithSecurityTokenDescriptor(SecurityTokenDescriptor securityTokenDescriptor)
    {
        _securityTokenDescriptor = securityTokenDescriptor;
        return this;
    }

    public JwtToken Build()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(_securityTokenDescriptor);
        return new(
            tokenHandler.WriteToken(token), 
            _securityTokenDescriptor?.Expires ?? throw new ArgumentNullException(nameof(_securityTokenDescriptor.Expires))
        );
    }
}