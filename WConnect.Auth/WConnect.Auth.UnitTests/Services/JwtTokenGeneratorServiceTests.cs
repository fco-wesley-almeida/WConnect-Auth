using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using WConnect.Auth.Application.Exceptions;
using WConnect.Auth.Application.Services;
using WConnect.Auth.Core.UseCases.SignIn;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.UnitTests.CustomFakers;
using WConnect.Auth.UnitTests.Fixtures;
using DateTime = System.DateTime;

namespace WConnect.Auth.UnitTests.Services;

public class JwtTokenGeneratorServiceTests: IClassFixture<UserFixture>
{
    private readonly Mock<IClaimsIdentityBuilder> _mockClaimsIdentityBuilder;
    private readonly Mock<IJwtTokenBuilder> _mockJwtTokenBuilder;
    private readonly Mock<ISecurityTokenDescriptorBuilder> _mockSecurityTokenDescriptorBuilder;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly User _user;
    private readonly IJwtTokenGeneratorService _sut;
    private readonly TimeFaker _timeFaker;

    private const int ValidityInMinutes = 15;
    private const string Key = "NjQ2OWEwMzIwMGQxNDZkYzkzNjA0MDU2YzI2Yjk4ZjI=";

    public JwtTokenGeneratorServiceTests(UserFixture userFixture)
    {
        _user = userFixture.User;
        _mockClaimsIdentityBuilder = new();
        _mockJwtTokenBuilder = new();
        _mockSecurityTokenDescriptorBuilder = new();
        _mockConfiguration = new();
        _timeFaker = new(DateTime.Now);
        _sut = new JwtTokenGeneratorService(
            configuration: _mockConfiguration.Object,
            claimsIdentityBuilder: _mockClaimsIdentityBuilder.Object,
            jwtTokenBuilder: _mockJwtTokenBuilder.Object,
            securityTokenDescriptorBuilder: _mockSecurityTokenDescriptorBuilder.Object,
            timeProvider: _timeFaker
        );
        
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithAudience(It.IsAny<string>()))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithExpires(It.IsAny<DateTime>()))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithIssuer(It.IsAny<string>()))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithSubject(It.IsAny<ClaimsIdentity>()))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithSigningCredentials(It.IsAny<SigningCredentials>()))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);

        _mockClaimsIdentityBuilder
            .Setup(x => x.AddClaim(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(_mockClaimsIdentityBuilder.Object);
        _mockClaimsIdentityBuilder
            .Setup(x => x.Build())
            .Returns(new ClaimsIdentity());

        _mockJwtTokenBuilder
            .Setup(x => x.WithSecurityTokenDescriptor(It.IsAny<SecurityTokenDescriptor>()))
            .Returns(_mockJwtTokenBuilder.Object);
        _mockJwtTokenBuilder
            .Setup(x => x.Build())
            .Returns(new JwtToken("accessToken", DateTime.Now));
        
        
        _mockConfiguration
            .SetupGet(x => x["Jwt:TokenValidityInMinutes"])
            .Returns(ValidityInMinutes.ToString());
        _mockConfiguration
            .SetupGet(x => x["Jwt:Audience"])
            .Returns("audience");
        _mockConfiguration
            .SetupGet(x => x["Jwt:Issuer"])
            .Returns("issuer");
        _mockConfiguration
            .SetupGet(x => x["Jwt:Key"])
            .Returns(Key);

    }

    [Fact]
    public void GenerateToken_WhenTokenValidityInMinutesIsNotProvidedInConfiguration_ShouldThrowMissingConfigurationParameterException()
    {
        _mockConfiguration
            .SetupGet(x => x["Jwt:TokenValidityInMinutes"])
            .Returns((string?) null);
        var exception = Assert.Throws<MissingConfigurationParameterException>(() =>
        {
            _ = _sut.GenerateToken(_user);
        });
        Assert.Contains("Jwt:TokenValidityInMinutes", exception.Status.Detail);
    }
    
    [Fact]
    public void GenerateToken_WhenTokenValidityInMinutesIsNotAInt_ShouldThrowFormatException()
    {
        _mockConfiguration
            .SetupGet(x => x[It.IsAny<string>()])
            .Returns("NOT_AN_INT");
        Assert.Throws<FormatException>(() =>
        {
            _ = _sut.GenerateToken(_user);
        });
    }
    
    [Theory]
    [InlineData("2147483648")]
    [InlineData("-2147483649")]
    public void GenerateToken_WhenTokenValidityInMinutesIsOverflowedInt_ShouldThrowFormatException(string overflowedInt)
    {
        _mockConfiguration
            .SetupGet(x => x[It.IsAny<string>()])
            .Returns(overflowedInt);
        Assert.Throws<OverflowException>(() =>
        {
            _ = _sut.GenerateToken(_user);
        });
    }
    
    [Fact]
    public void GenerateToken_WhenAudienceIsNotProvided_ShouldThrowMissingConfigurationParameterException()
    {
        _mockConfiguration
            .SetupGet(x => x["Jwt:Audience"])
            .Returns((string?) null);
        var exception = Assert.Throws<MissingConfigurationParameterException>(() =>
        {
            _ = _sut.GenerateToken(_user);
        });
        Assert.Contains("Jwt:Audience", exception.Status.Detail);
    }
    
    [Fact]
    public void GenerateToken_WhenIssuerIsNotProvided_ShouldThrowMissingConfigurationParameterException()
    {
        _mockConfiguration
            .SetupGet(x => x["Jwt:Issuer"])
            .Returns((string?) null);
        var exception = Assert.Throws<MissingConfigurationParameterException>(() =>
        {
            _ = _sut.GenerateToken(_user);
        });
        Assert.Contains("Jwt:Issuer", exception.Status.Detail);
    }
    
    
    [Fact]
    public void GenerateToken_WhenKeyIsNotProvided_ShouldThrowMissingConfigurationParameterException()
    {
        _mockConfiguration
            .SetupGet(x => x["Jwt:Key"])
            .Returns((string?) null);
        var exception = Assert.Throws<MissingConfigurationParameterException>(() =>
        {
            _ = _sut.GenerateToken(_user);
        });
        Assert.Contains("Jwt:Key", exception.Status.Detail);
    }
    
    [Fact]
    public void GenerateToken_WhenAudienceIsProvided_ShouldCallMethodToAddAudience()
    {
        string expectedAudience = Faker.StringFaker.AlphaNumeric(10);
        _mockConfiguration
            .SetupGet(x => x["Jwt:Audience"])
            .Returns(expectedAudience);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithAudience(It.IsAny<string>()))
            .Callback(new Action<string>(actualAudience =>
            {
                Assert.Equal(expectedAudience, actualAudience);
            }))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_tokenValidityInMinutesIsProvided_ShouldCallMethodToAddExpires()
    {
        DateTime expectedExpiryTime = _timeFaker.Now()
            .ToUniversalTime()
            .AddMinutes(ValidityInMinutes);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithExpires(It.IsAny<DateTime>()))
            .Callback(new Action<DateTime>(actualExpiryTime =>
            {
                Assert.Equal(expectedExpiryTime.ToBinary(), actualExpiryTime.ToBinary());
            }))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_WhenIssuerIsProvided_ShouldCallMethodToAddIssuer()
    {
        string expectedIssuer = Faker.StringFaker.AlphaNumeric(10);
        _mockConfiguration
            .SetupGet(x => x["Jwt:Issuer"])
            .Returns(expectedIssuer);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithIssuer(It.IsAny<string>()))
            .Callback(new Action<string>(actualIssuer =>
            {
                Assert.Equal(expectedIssuer, actualIssuer);
            }))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_Always_ShouldAddClaimSubWithUserLoginValue()
    {
        _mockClaimsIdentityBuilder
            .Setup(x => x.AddClaim(JwtRegisteredClaimNames.Sub, It.IsAny<string>()))
            .Callback(new Action<string, string>((_, value) =>
            {
                Assert.Equal(_user.Credential.Login.ToString(), value);
            }))
            .Returns(_mockClaimsIdentityBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_Always_ShouldAddClaimJtiWithRandomGuid()
    {
        _mockClaimsIdentityBuilder
            .Setup(x => x.AddClaim(JwtRegisteredClaimNames.Jti, It.IsAny<string>()))
            .Callback(new Action<string, string>((_, value) =>
            {
                bool isGuid;
                try
                {
                    Guid.Parse(value);
                    isGuid = true;
                }
                catch
                {
                    isGuid = false;
                }
                Assert.True(isGuid);
            }))
            .Returns(_mockClaimsIdentityBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_Always_ShouldAddClaimContextWithJwtContextSerialized()
    {
        _mockClaimsIdentityBuilder
            .Setup(x => x.AddClaim("context", It.IsAny<string>()))
            .Callback(new Action<string, string>((_, value) =>
            {
                var jwtContext = new JwtContext(_user);
                Assert.Equal(JsonConvert.SerializeObject(jwtContext), value);
            }))
            .Returns(_mockClaimsIdentityBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_Always_ShouldClaimsIdentityBeProvidedToMethodWithSubject()
    {
        ClaimsIdentity expectedClaimsIdentity = new();
        _mockClaimsIdentityBuilder
            .Setup(x => x.Build())
            .Returns(expectedClaimsIdentity);
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithSubject(It.IsAny<ClaimsIdentity>()))
            .Callback(new Action<ClaimsIdentity>(actualClaimsIdentity =>
            {
                Assert.Equal(expectedClaimsIdentity, actualClaimsIdentity);
            }))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_Always_ShouldSigningCredentialsBeProvidedToMethodWithSigningCredentials()
    {
        SigningCredentials expectedSigningCredentials = new(
            key: new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(Key)
            ),
            algorithm: SecurityAlgorithms.HmacSha512Signature
        );
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.WithSigningCredentials(It.IsAny<SigningCredentials>()))
            .Callback(new Action<SigningCredentials>(actualSigningCredentials =>
            {
                Assert.Equal(expectedSigningCredentials.Key.ToString(), actualSigningCredentials.Key.ToString());
                Assert.Equal(expectedSigningCredentials.Algorithm, actualSigningCredentials.Algorithm);
            }))
            .Returns(_mockSecurityTokenDescriptorBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_Always_ShouldSecurityTokenDescriptorBeProvidedToJwtTokenBuilder()
    {
        SecurityTokenDescriptor expectedSigningCredentials = new()
        {
            Audience = "audience",
            Issuer = "issuer"
        };
        _mockSecurityTokenDescriptorBuilder
            .Setup(x => x.Build())
            .Returns(expectedSigningCredentials);
        _mockJwtTokenBuilder
            .Setup(x => x.WithSecurityTokenDescriptor(It.IsAny<SecurityTokenDescriptor>()))
            .Callback(new Action<SecurityTokenDescriptor>(actualSecurityTokenDescriptor =>
            {
                Assert.Equal(expectedSigningCredentials, actualSecurityTokenDescriptor);
            }))
            .Returns(_mockJwtTokenBuilder.Object);
        _ = _sut.GenerateToken(_user);
    }
    
    [Fact]
    public void GenerateToken_Always_ShouldJwtTokenBuilderBeReturned()
    {
        JwtToken expectedJwtToken = new JwtToken("acess_token", DateTime.Now);
        _mockJwtTokenBuilder
            .Setup(x => x.Build())
            .Returns(expectedJwtToken);
        var actualJwtToken = _sut.GenerateToken(_user);
        Assert.Equal(expectedJwtToken, actualJwtToken);
    }
}