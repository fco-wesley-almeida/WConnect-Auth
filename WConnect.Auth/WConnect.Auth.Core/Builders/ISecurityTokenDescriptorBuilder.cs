using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WConnect.Auth.Core.Builders;

public interface ISecurityTokenDescriptorBuilder
{
    ISecurityTokenDescriptorBuilder WithSubject(ClaimsIdentity subject);
    ISecurityTokenDescriptorBuilder WithExpires(DateTime expires);
    ISecurityTokenDescriptorBuilder WithIssuer(string? issuer);
    ISecurityTokenDescriptorBuilder WithAudience(string? audience);
    ISecurityTokenDescriptorBuilder WithSigningCredentials(byte[] key, string algorithm);
    SecurityTokenDescriptor Build();
}