using WConnect.Auth.Core.DbModels;
using WConnect.Auth.Core.Providers;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;
using WConnect.Auth.UnitTests.Fixtures;

namespace WConnect.Auth.UnitTests.DatabaseModels;

public class UserRowTests: IClassFixture<UserFixture>
{
    private readonly User _user;
    private readonly ITimeProvider _timeProvider;
    
    public UserRowTests(UserFixture userFixture)
    {
        _user = userFixture.User;
        _timeProvider = new TimeFaker(DateTime.Now);
    }

    [Fact]
    public void Constructor_FromUser_SetsPropertiesCorrectly()
    {
        // Act
        var userRow = new UserRow(_user, _timeProvider);

        // Assert
        Assert.Equal(0, userRow.Id);
        Assert.Equal(_user.PersonalData.Name, userRow.Name);
        Assert.Equal(_user.PersonalData.PhotoUrl?.ToString(), userRow.PhotoUrl);
        Assert.Equal(_user.Credential.Login.ToString(), userRow.Login);
        Assert.Equal(_user.Credential.Password.ToString(), userRow.Password);
        Assert.Equal(_timeProvider.Now().ToBinary(), userRow.CreatedAt.ToBinary());
        Assert.Equal(_timeProvider.Now().ToBinary(), userRow.ModifiedAt.ToBinary());
        Assert.False(userRow.Deleted);
    }
}