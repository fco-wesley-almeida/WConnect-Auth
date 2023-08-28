using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.UnitTests.ValueObjects;

public class CredentialTests
{
    private readonly Faker<Password> _passwordFaker = new();
    private readonly Faker<Login> _loginFaker = new();

    [Fact]
    public void Constructor_WhenLoginIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Credential(null!, _passwordFaker.Generate());
        });
    }
    
    [Fact]
    public void Constructor_WhenPasswordIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Credential(_loginFaker.Generate(), null!);
        });
    }
    
    [Fact]
    public void Constructor_WhenArgumentsAreValid_ShouldPass()
    {
        _ = new Credential(_loginFaker.Generate(), _passwordFaker.Generate());
        Assert.True(true);
    }
}