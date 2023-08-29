using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;

namespace WConnect.Auth.UnitTests.Fixtures;

public class UserFixture
{
    public readonly User User;

    public UserFixture()
    {
        Credential credential = new(
            id: new Random().Next(),
            login: new Login(LoginFaker.Fake()),
            password: new Password(PasswordFaker.Fake())
        );
        PersonalData personalData = new(
            name: Faker.StringFaker.AlphaNumeric(10),
            photoUrl: new Uri(Faker.InternetFaker.Url())
        );
        User = new User(credential, personalData);
    }
}