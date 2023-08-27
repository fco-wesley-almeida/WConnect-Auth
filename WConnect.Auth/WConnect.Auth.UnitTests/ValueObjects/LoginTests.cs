using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.UnitTests.ValueObjects;

public class LoginTests
{
    private const int MinLength = 5;
    private const int MaxLength = 20;
    
    [Fact]
    public void Constructor_LoginIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Login (null!);
        });
    }
    
    [Fact]
    public void Constructor_WhenLoginIsEmpty_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new Login ("");
        });
    }
    
    [Fact]
    public void Constructor_WhenLoginLengthLesserThanMinLength_ShouldThrowArgumentOutOfRangeException()
    {
        string login = Faker.StringFaker.AlphaNumeric(MinLength - 1);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new Login(login);
        });
    }
    
    [Fact]
    public void Constructor_WhenLoginLengthGreaterThanMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        string login = Faker.StringFaker.AlphaNumeric(MaxLength + 1);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new Login(login);
        });
    }
    
    [Theory]
    [InlineData(MinLength)]
    [InlineData(MinLength + 1)]
    [InlineData(MaxLength)]
    [InlineData(MaxLength - 1)]
    public void Constructor_WhenLoginLengthIsValid_ShouldPass(int length)
    {
        string login = Faker.StringFaker.AlphaNumeric(length);
        _ = new Login(login);
        Assert.True(true);
    }
    
    [Fact]
    public void Constructor_WhenLoginIsBetweenSpaces_ShouldTrim()
    {
        string login = " " + Faker.StringFaker.AlphaNumeric(MinLength) + " ";
        var loginObj = new Login(login);
        Assert.Equal(login.Trim(), loginObj.ToString());
    }
}