using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;

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
        Login login = _loginFaker.Generate();
        Password password = _passwordFaker.Generate();
        Credential credential = new(login, password);
        Assert.Equal(password.ToString(), credential.Password.ToString());
        Assert.Equal(login.ToString(), credential.Login.ToString());
    }
    
    [Fact]
    public void Constructor_WhenIdIsProvidedAndLoginIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Credential(1, null!, _passwordFaker.Generate());
        });
    }
    
    [Fact]
    public void Constructor_WhenIdIsProvidedAndPasswordIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Credential(1, _loginFaker.Generate(), null!);
        });
    }
    
    [Fact]
    public void Constructor_WhenIdIsProvidedAndArgumentsAreValid_ShouldPass()
    {
        int id = new Random().Next();
        Login login = _loginFaker.Generate();
        Password password = _passwordFaker.Generate();
        Credential credential = new(id, login, password);
        Assert.Equal(id, credential.Id ?? 0);
        Assert.Equal(password.ToString(), credential.Password.ToString());
        Assert.Equal(login.ToString(), credential.Login.ToString());
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WhenIdIsLesserThanZero_ShouldThrowArgumentOutOfRangeException(int id)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new Credential(id, _loginFaker.Generate(), _passwordFaker.Generate());
        });
    }
}