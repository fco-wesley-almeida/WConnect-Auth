using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;

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
    
    [Fact]
    public void Constructor_WithoutParams_ShouldWork()
    {
        _ = new Password();
        Assert.True(true);
    }
    
    [Fact]
    public void Equals_WhenPasswordIsDifferent_ShouldReturnFalse()
    {
        Password password1 = new(PasswordFaker.Fake());
        Password password2 = new(PasswordFaker.Fake());
        Assert.False(password1.Equals(password2));
    }
    
    [Fact]
    public void Equals_WhenPasswordIsEqual_ShouldReturnTrue()
    {
        string fakePassword = PasswordFaker.Fake();
        Password password1 = new(fakePassword);
        Password password2 = new(fakePassword);
        Assert.True(password1.Equals(password2));
    }
}