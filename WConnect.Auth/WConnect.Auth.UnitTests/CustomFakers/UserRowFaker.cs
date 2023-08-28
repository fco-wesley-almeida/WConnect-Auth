using WConnect.Auth.Core.DbModels;

namespace WConnect.Auth.UnitTests.CustomFakers;

public class UserRowFaker
{
    public static UserRow Fake(bool deleted)
    {
        return new UserRow(
            id: 0,
            name: Faker.StringFaker.AlphaNumeric(10),
            login: Faker.StringFaker.AlphaNumeric(10),
            password: Faker.StringFaker.AlphaNumeric(10),
            photoUrl: Faker.StringFaker.AlphaNumeric(10),
            createdAt: Faker.DateTimeFaker.DateTime(),
            modifiedAt: Faker.DateTimeFaker.DateTime(),
            deleted
        );
    }
}