using System.IdentityModel.Tokens.Jwt;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace WConnect.Auth.UnitTests.CustomFakers;

public class SecurityTokenHandlerFake: JwtSecurityTokenHandler
{
    private class FakeSecurityToken : SecurityToken
    {
        public override string Id { get; } = null!;
        public override string Issuer { get; } = null!;
        public override SecurityKey SecurityKey { get; } = null!;
        public override SecurityKey SigningKey { get; set; } = null!;
        public override DateTime ValidFrom { get; } = DateTime.Now;
        public override DateTime ValidTo { get; } = DateTime.Now;
    }
    
    private readonly string _fakeToken;

    public SecurityTokenHandlerFake(string fakeToken)
    {
        _fakeToken = fakeToken;
    }


    public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
    {
        return new FakeSecurityToken();
    }

    public override string WriteToken(SecurityToken token)
    {
        return _fakeToken;
    }
}