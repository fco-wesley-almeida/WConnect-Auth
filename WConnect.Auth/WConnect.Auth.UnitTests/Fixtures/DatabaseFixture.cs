using System.Data;
using Dapper;
using MySqlConnector;
using WConnect.Auth.Core.DbModels;
using WConnect.Auth.UnitTests.CustomFakers;

namespace WConnect.Auth.UnitTests.Fixtures;

public class DatabaseFixture
{
	public IDbConnection Connection { get; private set; }
	public UserRow[] Users { get; }
	public const int UsersCount = 100;
	
    public DatabaseFixture()
    {
	    Users = new UserRow[UsersCount];
	    Connection = new MySqlConnection("Server=localhost;User ID=m01;Password=m01;Database=wconnect-auth");
    }

    public void CreatePreConditionsForUserTesting()
    {
        const string sql = @"
			INSERT INTO `user`
			(
				name,
				login,
				password,
				photo_url,
				deleted,
				created_at,
				modified_at
			)
			VALUES
			(
				@Name,
				@Login,
				@Password,
				@PhotoUrl,
				@Deleted,
				@CreatedAt,
				@ModifiedAt
			);
        ";
        Connection.Execute("DELETE FROM `user` WHERE 1");
        for (int i = 0; i < UsersCount; i++)
        {
	        var user = UserRowFaker.Fake((i & 1) == 0);
	        Connection.Execute(sql, user);
	        Users[i] = user;
        }
    }
}