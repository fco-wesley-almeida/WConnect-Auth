using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.UnitTests.ValueObjects;

public class PersonalDataTests
{
    [Fact]
    public void Constructor_NameIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new PersonalData(null!, new Uri(Faker.InternetFaker.Url()));
        });
    }
    
    [Fact]
    public void Constructor_WhenNameIsEmpty_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new PersonalData("", new Uri(Faker.InternetFaker.Url()));
        });
    }
    
    [Fact]
    public void Constructor_WhenPhotoUrlIsNull_ShouldPass()
    {
        _ = new PersonalData(Faker.StringFaker.AlphaNumeric(10), null);
        Assert.True(true);
    }
    
    [Fact]
    public void Constructor_WhenPhotoUrlIsNotNull_ShouldPass()
    {
        _ = new PersonalData(
            name: Faker.StringFaker.AlphaNumeric(10), 
            photoUrl: new Uri(Faker.InternetFaker.Url())
        );
        Assert.True(true);
    }
}