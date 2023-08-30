using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WConnect.Auth.Core.Builders;

namespace WConnect.Auth.Application.Builders;

public class SecurityTokenDescriptorBuilder: ISecurityTokenDescriptorBuilder
{
    private ClaimsIdentity _subject = null!;
    private string _issuer = null!;
    private string _audience = null!;
    private SigningCredentials _signingCredentials = null!;
    private DateTime? _expires;

    public ISecurityTokenDescriptorBuilder WithSubject(ClaimsIdentity subject)
    {
        _subject = subject;
        return this;
    }

    public ISecurityTokenDescriptorBuilder WithIssuer(string issuer)
    {
        _issuer = issuer;
        return this;
    }
    
    public ISecurityTokenDescriptorBuilder WithAudience(string audience)
    {
        _audience = audience;
        return this;
    }
    
    public ISecurityTokenDescriptorBuilder WithSigningCredentials(SigningCredentials signingCredentials)
    {
        _signingCredentials = signingCredentials;
        return this;
    }
    
    public ISecurityTokenDescriptorBuilder WithExpires(DateTime expires)
    {
        _expires = expires;
        return this;
    }

    public SecurityTokenDescriptor Build()
    {
        ArgumentNullException.ThrowIfNull(_subject);
        ArgumentNullException.ThrowIfNull(_issuer);
        ArgumentNullException.ThrowIfNull(_audience);
        ArgumentNullException.ThrowIfNull(_signingCredentials);
        return new SecurityTokenDescriptor
        {
            Subject = _subject,
            Expires = _expires ?? throw new ArgumentNullException(nameof(_expires)),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = _signingCredentials
        };
    }
}