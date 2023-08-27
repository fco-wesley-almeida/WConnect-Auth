using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.UnitTests.ValueObjects;

public class PasswordTests
{
    private const int MinLength = 5;
    private const int MaxLength = 100;
    
    [Fact]
    public void Constructor_PasswordIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new Password (null!);
        });
    }
    
    [Fact]
    public void Constructor_WhenPasswordIsEmpty_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new Password ("");
        });
    }
    
    [Fact]
    public void Constructor_WhenPasswordLengthLesserThanMinLength_ShouldThrowArgumentOutOfRangeException()
    {
        string password = Faker.StringFaker.AlphaNumeric(MinLength - 1);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new Password(password);
        });
    }
    
    [Fact]
    public void Constructor_WhenPasswordLengthGreaterThanMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        string password = Faker.StringFaker.AlphaNumeric(MaxLength + 1);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new Password(password);
        });
    }
    
    [Theory]
    [InlineData(MinLength)]
    [InlineData(MinLength + 1)]
    [InlineData(MaxLength)]
    [InlineData(MaxLength - 1)]
    public void Constructor_WhenPasswordLengthIsValid_ShouldPass(int length)
    {
        string password = Faker.StringFaker.AlphaNumeric(length);
        _ = new Password(password);
        Assert.True(true);
    }
    
    [Fact]
    public void Constructor_WhenPasswordIsBetweenSpaces_ShouldTrim()
    {
        string password = " " + Faker.StringFaker.AlphaNumeric(MinLength) + " ";
        var passwordObj = new Password(password);
        Assert.Equal(password.Trim(), passwordObj.ToString());
    }
}