namespace WConnect.Auth.UnitTests.CustomFakers;

public class PasswordFaker
{
    public static string Fake()
    {
        return Faker.StringFaker.AlphaNumeric(10);
    }
}