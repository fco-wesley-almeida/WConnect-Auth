using System.Data;
using WConnect.Auth.Database.Repositories;
using WConnect.Auth.Domain.ValueObjects;
using WConnect.Auth.UnitTests.CustomFakers;
using WConnect.Auth.UnitTests.Fixtures;
using Dapper;
using WConnect.Auth.Core.Database;
using WConnect.Auth.Core.Database.PersistenceModels;

namespace WConnect.Auth.UnitTests.Repositories;

public class UserRepositoryTests: IClassFixture<DatabaseFixture>
{
    private UserRow[] _users;
    private IDbConnection _connection;
    private IUserRepository _sut;
    private readonly DatabaseFixture _databaseFixture;
    public UserRepositoryTests(DatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
        _users = databaseFixture.Users;
        _connection = databaseFixture.Connection;
        _sut = new UserRepository(_connection);
        _databaseFixture.CreatePreConditionsForUserTesting();
    }

    [Fact]
    public async Task FindUserByLoginAsync_WhenLoginDoesNotExists_ShouldReturnNull()
    {
        Login login = new Login(LoginFaker.Fake());
        UserRow? userRow = await _sut.FindUserByLoginAsync(login);
        Assert.Null(userRow);
    }
    
    [Fact]
    public async Task FindUserByLoginAsync_WhenLoginExistsButUserIsDeleted_ShouldReturnNull()
    {
        Login login = new Login(_users.First(x => x.Deleted).Login);
        UserRow? userRow = await _sut.FindUserByLoginAsync(login);
        Assert.Null(userRow);
    }
    
    
    [Fact]
    public async Task FindUserByLoginAsync_WhenLoginExistsAndIsNotDeleted_ShouldReturn()
    {
        Login login = new Login(_users.First(x => !x.Deleted).Login);
        UserRow? userRow = await _sut.FindUserByLoginAsync(login);
        Assert.NotNull(userRow);
    }
    
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task InsertAsync_Always_ShouldBePersisted(bool deleted)
    {
        UserRow userRow = UserRowFaker.Fake(deleted);
        int userId = await _sut.InsertAsync(userRow);
        UserRow userPersisted = await _connection.QueryFirstOrDefaultAsync<UserRow>(@$"
            select 
                id, login, name, password, photo_url PhotoUrl, created_at CreatedAt, modified_at ModifiedAt, deleted 
            from 
                user 
            where id = {userId}
        ");
        Assert.NotNull(userPersisted);
        Assert.Equal(userRow.Login, userPersisted.Login);
        Assert.Equal(userRow.Name, userPersisted.Name);
        Assert.Equal(userRow.Password, userPersisted.Password);
        Assert.Equal(userRow.PhotoUrl, userPersisted.PhotoUrl);
        Assert.Equal(
            expected: Utils.DateFormatToIso8601(userRow.CreatedAt), 
            actual: Utils.DateFormatToIso8601(userPersisted.CreatedAt)
        );
        Assert.Equal(
            expected: Utils.DateFormatToIso8601(userRow.ModifiedAt), 
            actual: Utils.DateFormatToIso8601(userPersisted.ModifiedAt)
        );
        Assert.Equal(userRow.Deleted, userPersisted.Deleted);
    }
}