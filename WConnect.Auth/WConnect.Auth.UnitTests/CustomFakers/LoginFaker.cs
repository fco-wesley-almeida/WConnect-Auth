namespace WConnect.Auth.UnitTests.CustomFakers;

public class LoginFaker
{
    public static string Fake()
    {
        return Faker.StringFaker.AlphaNumeric(10);
    }
}