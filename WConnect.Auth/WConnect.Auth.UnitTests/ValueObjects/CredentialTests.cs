using System.Net.Http.Json;
using Newtonsoft.Json;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.UnitTests.ValueObjects;

public class CredentialTests
{
    private Faker<Password> PasswordFaker = new();
    private Faker<Login> LoginFaker = new();

    [Fact]
    public void Constructor_WhenLoginIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Credential(null!, PasswordFaker.Generate());
        });
    }
    
    [Fact]
    public void Constructor_WhenPasswordIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Credential(LoginFaker.Generate(), null!);
        });
    }
    
    [Fact]
    public void Constructor_WhenArgumentsAreValid_ShouldPass()
    {
        _ = new Credential(LoginFaker.Generate(), PasswordFaker.Generate());
        Assert.True(true);
    }
}