using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using WConnect.Auth.Application.Builders;
using WConnect.Auth.Core.UseCases.SignIn;
using WConnect.Auth.UnitTests.CustomFakers;

namespace WConnect.Auth.UnitTests.Builders;

public class JwtTokenBuilderTests
{
    private IJwtTokenBuilder _sut = null!;
    
    [Fact]
    public void Build_WhenSecurityTokenDescriptorIsNotProvided_ShouldThrowArgumentNullException()
    {
        _sut = new JwtTokenBuilder(new JwtSecurityTokenHandler());
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = _sut.Build();
        });
    }
    
    [Fact]
    public void Build_WhenSecurityTokenDescriptorIsProvidedAndExpiresIsNull_ShouldThrowArgumentNullException()
    {
        _sut = new JwtTokenBuilder(new JwtSecurityTokenHandler());
        SecurityTokenDescriptor securityTokenDescriptor = new();
        _sut.WithSecurityTokenDescriptor(securityTokenDescriptor);
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = _sut.Build();
        });
    }
    
    
    [Fact]
    public void Build_WhenSecurityTokenDescriptorIsProvidedAndExpiresIsNotNull_ShouldPass()
    {
        string fakeToken = Faker.StringFaker.AlphaNumeric(10);
        _sut = new JwtTokenBuilder(new SecurityTokenHandlerFake(fakeToken));
        DateTime expires = DateTime.Now.AddMinutes(new Random().Next() % 1440);
        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Expires = DateTime.Now.AddYears(1),
        };
        securityTokenDescriptor.Expires = expires;
        _sut.WithSecurityTokenDescriptor(securityTokenDescriptor);
        var jwtToken = _sut.Build();
        Assert.Equal(jwtToken.AccessTokenExpiryTime.ToBinary(), securityTokenDescriptor.Expires.Value.ToBinary());
        Assert.Equal(fakeToken, jwtToken.AccessToken);
    }
}