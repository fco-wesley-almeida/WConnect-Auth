using WConnect.Auth.Core.ApplicationsModels;

namespace WConnect.Auth.UnitTests.ApplicationModels;

public class JwtTokenTests
{
    [Fact]
    public void Constructor_WhenAccessTokenIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new JwtToken(null!, DateTime.Now);
        });
    }
    
    
    [Fact]
    public void Constructor_WhenAccessTokenIsEmpty_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new JwtToken("", DateTime.Now);
        });
    }
    
    
    [Fact]
    public void Constructor_WhenAccessTokenIsNotNullOrEmpty_ShouldPass()
    {
        var accessToken = Faker.StringFaker.AlphaNumeric(10);
        var dateTime = DateTime.Now;
        var jwtToken = new JwtToken(accessToken!, dateTime);
        Assert.Equal(accessToken, jwtToken.AccessToken);
        Assert.Equal(dateTime.ToBinary(), jwtToken.AccessTokenExpiryTime.ToBinary());
    }
}