using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using WConnect.Auth.Core.UseCases.SignIn;

namespace WConnect.Auth.Application.Builders;

public class JwtTokenBuilder: IJwtTokenBuilder
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private SecurityTokenDescriptor _securityTokenDescriptor = null!;

    public JwtTokenBuilder(JwtSecurityTokenHandler jwtSecurityTokenHandler)
    {
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public IJwtTokenBuilder WithSecurityTokenDescriptor(SecurityTokenDescriptor securityTokenDescriptor)
    {
        _securityTokenDescriptor = securityTokenDescriptor;
        return this;
    }

    public JwtToken Build()
    {
        ArgumentNullException.ThrowIfNull(_securityTokenDescriptor);
        var token = _jwtSecurityTokenHandler.CreateToken(_securityTokenDescriptor);
        return new(
            _jwtSecurityTokenHandler.WriteToken(token), 
            _securityTokenDescriptor.Expires 
                ?? throw new ArgumentNullException(nameof(_securityTokenDescriptor.Expires))
        );
    }
}