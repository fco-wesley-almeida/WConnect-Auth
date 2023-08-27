using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.UnitTests.Entities;

public class UserTests
{
    private readonly Faker<Credential> _credentialFaker = new();
    private readonly Faker<PersonalData> _personalDataFaker = new();
    
    [Fact]
    public void Constructor_WhenCredentialIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new User(null!, _personalDataFaker.Generate());
        });
    }
    
    [Fact]
    public void Constructor_WhenPersonalDataIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new User(_credentialFaker.Generate(), null!);
        });
    }
    
    
    [Fact]
    public void Constructor_WhenArgumentsAreValid_ShouldPass()
    {
        _ = new User(_credentialFaker.Generate(), _personalDataFaker.Generate());
        Assert.True(true);
    }
}