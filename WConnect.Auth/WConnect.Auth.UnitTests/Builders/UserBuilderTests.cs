using WConnect.Auth.Application.Builders;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.UnitTests.CustomFakers;

namespace WConnect.Auth.UnitTests.Builders;

public class UserBuilderTests
{
    private IUserBuilder _sut;

    public UserBuilderTests()
    {
        _sut = new UserBuilder();
    }
    
    [Fact]
    public void Build_WhenLoginIsMissing_ShouldThrowArgumentNullException()
    {
        // Arrange
        string name = Faker.StringFaker.AlphaNumeric(10);
        string password = PasswordFaker.Fake();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
            _sut.WithName(name)
                .WithPassword(password)
                .Build());
    }
    
    [Fact]
    public void Build_WhenPasswordIsMissing_ShouldThrowArgumentNullException()
    {
        // Arrange
        string name = Faker.StringFaker.AlphaNumeric(10);
        string login = PasswordFaker.Fake();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
            _sut
                .WithName(name)
                .WithLogin(login)
                .Build());
    }
    
    
    [Fact]
    public void Build_WhenNameIsMissing_ShouldThrowArgumentNullException()
    {
        // Arrange
        string login = LoginFaker.Fake();
        string password = PasswordFaker.Fake();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() =>
            _sut
                .WithLogin(login)
                .WithPassword(password)
                .Build());
    }
    
    [Fact]
    public void Build_WhenPhotoUrlIsNull_ShouldPass()
    {
        // Arrange
        string login = LoginFaker.Fake();
        string password = PasswordFaker.Fake();
        string name = Faker.StringFaker.AlphaNumeric(10);

        // Act 
        _sut
            .WithLogin(login)
            .WithPassword(password)
            .WithName(name)
            .WithPhotoUrl(null)
            .Build();
        
        // Assert
        Assert.True(true);
    }
    
    [Fact]
    public void Build_WhenAllMethodsAreCalled_ShouldBuild()
    {
        // Arrange
        string name = Faker.StringFaker.AlphaNumeric(10);
        string login = LoginFaker.Fake();
        string password = PasswordFaker.Fake();
        var photoUri = new Uri(Faker.InternetFaker.Url());

        // Act
        var userDomain = _sut
            .WithName(name)
            .WithLogin(login)
            .WithPassword(password)
            .WithPhotoUrl(photoUri.ToString())
            .Build();

        // Assert
        Assert.Equal(null!, userDomain.Credential.Id);
        Assert.Equal(name, userDomain.PersonalData.Name);
        Assert.Equal(login, userDomain.Credential.Login.ToString());
        Assert.Equal(password, userDomain.Credential.Password.ToString());
        Assert.Equal(photoUri.ToString(), userDomain.PersonalData.PhotoUrl!.ToString());
    }
    
    [Fact]
    public void Build_WhenWithIdIsCalled_ShouldPass()
    {
        // Arrange
        string name = Faker.StringFaker.AlphaNumeric(10);
        string login = LoginFaker.Fake();
        string password = PasswordFaker.Fake();
        var photoUri = new Uri(Faker.InternetFaker.Url());
        int id = new Random().Next();

        // Act
        var userDomain = _sut
            .WithId(id)
            .WithName(name)
            .WithLogin(login)
            .WithPassword(password)
            .WithPhotoUrl(photoUri.ToString())
            .Build();

        // Assert
        Assert.Equal(id, userDomain.Credential.Id);
        Assert.Equal(name, userDomain.PersonalData.Name);
        Assert.Equal(login, userDomain.Credential.Login.ToString());
        Assert.Equal(password, userDomain.Credential.Password.ToString());
        Assert.Equal(photoUri.ToString(), userDomain.PersonalData.PhotoUrl!.ToString());
    }
}