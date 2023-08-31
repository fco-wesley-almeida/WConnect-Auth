using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Core.ApplicationsModels;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.Providers;
using WConnect.Auth.Core.Services;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Application.Configuration;

namespace WConnect.Auth.Application.Services;

public class JwtTokenGeneratorService: IJwtTokenGeneratorService
{
    private readonly IClaimsIdentityBuilder _claimsIdentityBuilder;
    private readonly IJwtTokenBuilder _jwtTokenBuilder;
    private readonly ISecurityTokenDescriptorBuilder _securityTokenDescriptorBuilder;
    private readonly ITimeProvider _timeProvider;
    private readonly IConfiguration _configuration;
    
    public JwtTokenGeneratorService(
        IConfiguration configuration, 
        IClaimsIdentityBuilder claimsIdentityBuilder, 
        IJwtTokenBuilder jwtTokenBuilder, 
        ISecurityTokenDescriptorBuilder securityTokenDescriptorBuilder, 
        ITimeProvider timeProvider
    )
    {
        _configuration = configuration;
        _claimsIdentityBuilder = claimsIdentityBuilder;
        _jwtTokenBuilder = jwtTokenBuilder;
        _securityTokenDescriptorBuilder = securityTokenDescriptorBuilder;
        _timeProvider = timeProvider;
    }

    public JwtToken GenerateToken(User user)
    {
        JwtContext jwtContext = new(user);
        return _jwtTokenBuilder
              .WithSecurityTokenDescriptor(SecurityTokenDescriptor(user, jwtContext))
              .Build();
    }

    private SecurityTokenDescriptor SecurityTokenDescriptor(User user, JwtContext jwtContext)
    {
        var tokenValidityInMinutes = _configuration.GetInt("Jwt:TokenValidityInMinutes");
        var expires = _timeProvider.Now()
            .ToUniversalTime()
            .AddMinutes(tokenValidityInMinutes);
        return _securityTokenDescriptorBuilder
            .WithAudience(_configuration.GetString("Jwt:Audience"))
            .WithExpires(expires)
            .WithIssuer(_configuration.GetString("Jwt:Issuer"))
            .WithSubject(ClaimsIdentity(user, jwtContext))
            .WithSigningCredentials(SigningCredentials())
            .Build();
    }

    private ClaimsIdentity ClaimsIdentity(User user, JwtContext jwtContext) =>
        _claimsIdentityBuilder
           .AddClaim(JwtRegisteredClaimNames.Sub, user.Credential.Login.ToString())
           .AddClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
           .AddClaim("context", JsonConvert.SerializeObject(jwtContext))
           .Build();
    
    private SigningCredentials SigningCredentials() => new (
        key: new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(_configuration.GetString("Jwt:Key"))
        ),
        algorithm: SecurityAlgorithms.HmacSha512Signature
    );
}