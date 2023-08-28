using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WConnect.Auth.Core.Builders;

namespace WConnect.Auth.Application.Builders;

public class SecurityTokenDescriptorBuilder: ISecurityTokenDescriptorBuilder
{
    private ClaimsIdentity? _subject;
    private DateTime? _expires;
    private string? _issuer;
    private string? _audience;
    private SigningCredentials? _signingCredentials;

    public ISecurityTokenDescriptorBuilder WithSubject(ClaimsIdentity subject)
    {
        _subject = subject;
        return this;
    }
    
    public ISecurityTokenDescriptorBuilder WithExpires(DateTime expires)
    {
        _expires = expires;
        return this;
    }
    
    public ISecurityTokenDescriptorBuilder WithIssuer(string? issuer)
    {
        _issuer = issuer;
        return this;
    }
    
    public ISecurityTokenDescriptorBuilder WithAudience(string? audience)
    {
        _audience = audience;
        return this;
    }
    
    public ISecurityTokenDescriptorBuilder WithSigningCredentials(byte[] key, string algorithm)
    {
        _signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature
        );
        return this;
    }

    public SecurityTokenDescriptor Build() =>
        new()
        {
            Subject = _subject,
            Expires = _expires,
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = _signingCredentials
        };
}