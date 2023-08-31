using WConnect.Auth.Core.ApplicationsModels;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;

namespace WConnect.Auth.UnitTests.ApplicationModels;

public class JwtContextTests
{ 
    [Fact]
    public void Constructor_WhenUserIdIsNull_ShouldThrowArgumentNullException()
    {
        Credential credential = new(
            login: new Login(LoginFaker.Fake()),
            password: new Password(PasswordFaker.Fake())
        );
        PersonalData personalData = new(
            name: Faker.StringFaker.AlphaNumeric(10),
            photoUrl: new Uri(Faker.InternetFaker.Url())
        );
        User user = new(credential, personalData);
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new JwtContext(user);
        });
    }
    
    [Fact]
    public void Constructor_WhenUserIdIsNotNull_ShouldThrowArgumentNullException()
    {
        int id = new Random().Next();
        Credential credential = new(
            id,
            login: new Login(LoginFaker.Fake()),
            password: new Password(PasswordFaker.Fake())
        );
        PersonalData personalData = new(
            name: Faker.StringFaker.AlphaNumeric(10),
            photoUrl: new Uri(Faker.InternetFaker.Url())
        );
        User user = new(credential, personalData);
        var jwtContext = new JwtContext(user);
        Assert.Equal(id, jwtContext.UserId);
    }
}