using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WConnect.Auth.Application.Builders;
using WConnect.Auth.Core.UseCases.SignIn;

namespace WConnect.Auth.UnitTests.Builders;

public class SecurityTokenDescriptorBuilderTests
{
    private ISecurityTokenDescriptorBuilder _sut;

    public SecurityTokenDescriptorBuilderTests()
    {
        _sut = new SecurityTokenDescriptorBuilder();
    }

    
    [Fact]
    public void Build_WhenSubjectIsNotProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        var audience = Faker.StringFaker.AlphaNumeric(10);
        var expires = Faker.DateTimeFaker.DateTime();
        var issuer = Faker.StringFaker.AlphaNumeric(10);
        var key = "NjQ2OWEwMzIwMGQxNDZkYzkzNjA0MDU2YzI2Yjk4ZjI="u8.ToArray();
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(key),
            algorithm: SecurityAlgorithms.HmacSha512Signature
        );

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = _sut
                // .WithSubject(subject)
                .WithIssuer(issuer)
                .WithAudience(audience)
                .WithSigningCredentials(signingCredentials)
                .WithExpires(expires)
                .Build();
        });
    }
    
    [Fact]
    public void Build_WhenIssuerIsNotProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        var audience = Faker.StringFaker.AlphaNumeric(10);
        var expires = Faker.DateTimeFaker.DateTime();
        var subject = new ClaimsIdentity();
        var key = "NjQ2OWEwMzIwMGQxNDZkYzkzNjA0MDU2YzI2Yjk4ZjI="u8.ToArray();
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(key),
            algorithm: SecurityAlgorithms.HmacSha512Signature
        );
        
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = _sut
                .WithSubject(subject)
                // .WithIssuer(issuer)
                .WithAudience(audience)
                .WithSigningCredentials(signingCredentials)
                .WithExpires(expires)
                .Build();
        });
    }
    
    [Fact]
    public void Build_WhenAudienceIsNotProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        var expires = Faker.DateTimeFaker.DateTime();
        var issuer = Faker.StringFaker.AlphaNumeric(10);
        var subject = new ClaimsIdentity();
        var key = "NjQ2OWEwMzIwMGQxNDZkYzkzNjA0MDU2YzI2Yjk4ZjI="u8.ToArray();
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(key),
            algorithm: SecurityAlgorithms.HmacSha512Signature
        );
        
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = _sut
                .WithSubject(subject)
                .WithIssuer(issuer)
                // .WithAudience(audience)
                .WithSigningCredentials(signingCredentials)
                .WithExpires(expires)
                .Build();
        });
    }
    
    
    [Fact]
    public void Build_WhenSigningCredentialsIsNotProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        var audience = Faker.StringFaker.AlphaNumeric(10);
        var expires = Faker.DateTimeFaker.DateTime();
        var issuer = Faker.StringFaker.AlphaNumeric(10);
        var subject = new ClaimsIdentity();
        
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = _sut
                .WithSubject(subject)
                .WithIssuer(issuer)
                .WithAudience(audience)
                // .WithSigningCredentials(signingCredentials)
                .WithExpires(expires)
                .Build();
        });
    }
    
    [Fact]
    public void Build_WhenExpiresIsNotProvided_ShouldThrowArgumentNullException()
    {
        // Arrange
        var audience = Faker.StringFaker.AlphaNumeric(10);
        var issuer = Faker.StringFaker.AlphaNumeric(10);
        var subject = new ClaimsIdentity();
        var key = "NjQ2OWEwMzIwMGQxNDZkYzkzNjA0MDU2YzI2Yjk4ZjI="u8.ToArray();
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(key),
            algorithm: SecurityAlgorithms.HmacSha512Signature
        );
        
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = _sut
                .WithSubject(subject)
                .WithIssuer(issuer)
                .WithAudience(audience)
                .WithSigningCredentials(signingCredentials)
                // .WithExpires(expires)
                .Build();
        });
    }
    
    
    [Fact]
    public void Build_WhenAllMethodsAreCalled_ShouldPass()
    {
        // Arrange
        var audience = Faker.StringFaker.AlphaNumeric(10);
        var expires = Faker.DateTimeFaker.DateTime();
        var issuer = Faker.StringFaker.AlphaNumeric(10);
        var subject = new ClaimsIdentity();
        var key = "NjQ2OWEwMzIwMGQxNDZkYzkzNjA0MDU2YzI2Yjk4ZjI="u8.ToArray();
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(key),
            algorithm: SecurityAlgorithms.HmacSha512Signature
        );
        
        // Act
        var securityTokenDescriptor = _sut
            .WithSubject(subject)
            .WithIssuer(issuer)
            .WithAudience(audience)
            .WithSigningCredentials(signingCredentials)
            .WithExpires(expires)
            .Build();
        
        // Assert
        Assert.Equal(audience, securityTokenDescriptor.Audience);
        Assert.Equal(expires.ToBinary(), securityTokenDescriptor.Expires!.Value.ToBinary());
        Assert.Equal(issuer, securityTokenDescriptor.Issuer);
        Assert.Equal(subject, securityTokenDescriptor.Subject);
        Assert.Equal(signingCredentials, securityTokenDescriptor.SigningCredentials);
    }
}