using WConnect.Auth.Application.Builders;
using WConnect.Auth.Core.Builders;

namespace WConnect.Auth.UnitTests.Builders;

public class ClaimsIdentityBuilderTests
{
    private IClaimsIdentityBuilder _sut = null!; 
    
    [Fact]
    public void Build_WhenAnyClaimIsAdded_ShouldClaimsIdentityBeEmpty()
    {
        _sut = new ClaimsIdentityBuilder();
        var claimsIdentity = _sut.Build();
        Assert.Empty(claimsIdentity.Claims);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void Build_WhenClaimsAreAdded_ShouldClaimsBeFullfilled(int numberClaims)
    {
        _sut = new ClaimsIdentityBuilder();
        var claims = new (string, string)[numberClaims];
        for (int i = 0; i < numberClaims; i++)
        {
            claims[i].Item1 = Faker.StringFaker.AlphaNumeric(10);
            claims[i].Item2 = Faker.StringFaker.AlphaNumeric(10);
            _sut.AddClaim(
                type: claims[i].Item1, 
                value: claims[i].Item2
            );
        }
        var claimsIdentity = _sut.Build();
        Assert.All(claimsIdentity.Claims, (claim, i) =>
        {
           Assert.Equal(claims[i].Item1, claim.Type);  
           Assert.Equal(claims[i].Item2, claim.Value);  
        });
    }
}