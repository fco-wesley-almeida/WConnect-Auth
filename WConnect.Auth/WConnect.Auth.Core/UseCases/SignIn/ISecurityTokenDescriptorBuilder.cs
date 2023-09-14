using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WConnect.Auth.Core.UseCases.SignIn;

public interface ISecurityTokenDescriptorBuilder
{
    ISecurityTokenDescriptorBuilder WithSubject(ClaimsIdentity subject);
    ISecurityTokenDescriptorBuilder WithExpires(DateTime expires);
    ISecurityTokenDescriptorBuilder WithIssuer(string issuer);
    ISecurityTokenDescriptorBuilder WithAudience(string audience);
    ISecurityTokenDescriptorBuilder WithSigningCredentials(SigningCredentials signingCredentials);
    SecurityTokenDescriptor Build();
}