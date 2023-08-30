using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WConnect.Auth.Core.ApplicationsModels;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.Services;
using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Application.Services;

public class JwtTokenGeneratorService: IJwtTokenGeneratorService
{
    private readonly byte[] _key;
    private readonly int _accessTokenValidityInMinutes;
    private readonly string _audience;
    private readonly string _issuer;
    private readonly IClaimsIdentityBuilder _claimsIdentityBuilder;
    private readonly IJwtTokenBuilder _jwtTokenBuilder;
    private readonly ISecurityTokenDescriptorBuilder _securityTokenDescriptorBuilder;
    
    public JwtTokenGeneratorService(
        IConfiguration configuration, 
        IClaimsIdentityBuilder claimsIdentityBuilder, 
        IJwtTokenBuilder jwtTokenBuilder, 
        ISecurityTokenDescriptorBuilder securityTokenDescriptorBuilder
    )
    {
        _claimsIdentityBuilder = claimsIdentityBuilder;
        _jwtTokenBuilder = jwtTokenBuilder;
        _securityTokenDescriptorBuilder = securityTokenDescriptorBuilder;
        _key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        _accessTokenValidityInMinutes = Convert.ToInt32(configuration["Jwt:TokenValidityInMinutes"]);
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
    }

    public JwtToken GenerateToken(User user)
    {
        JwtContext jwtContext = new(user);
        return _jwtTokenBuilder
              .WithSecurityTokenDescriptor(SecurityTokenDescriptor(user, jwtContext))
              .Build();
    }

    private SecurityTokenDescriptor SecurityTokenDescriptor(User user, JwtContext jwtContext) =>
        _securityTokenDescriptorBuilder
            .WithAudience(_audience)
            .WithExpires(DateTime.Now.ToUniversalTime().AddMinutes(_accessTokenValidityInMinutes))
            .WithIssuer(_issuer)
            .WithSubject(ClaimsIdentity(user, jwtContext))
            .WithSigningCredentials(SigningCredentials())
            .Build();

    private SigningCredentials SigningCredentials() => new (
        key: new SymmetricSecurityKey(_key),
        algorithm: SecurityAlgorithms.HmacSha512Signature
    );

    private ClaimsIdentity ClaimsIdentity(User user, JwtContext jwtContext) =>
        _claimsIdentityBuilder
           .AddClaim(JwtRegisteredClaimNames.Sub, user.Credential.Login.ToString())
           .AddClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
           .AddClaim("Context", JsonConvert.SerializeObject(jwtContext))
           .Build();
}