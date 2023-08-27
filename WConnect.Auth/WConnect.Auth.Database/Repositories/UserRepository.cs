using System.Data;
using Dapper;
using WConnect.Auth.Core.DbModels;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Database.Repositories;

public class UserRepository: IUserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<UserRow?> FindUserByLoginAsync(Login login)
    {
        const string sql = @"
			SELECT
				id 			AS Id,
				name 		AS Name,
				login 		AS Login,
				password 	AS Password,
				photo_url 	AS PhotoUrl,
				deleted 	AS Deleted,
				created_at 	AS CreatedAt,
				modified_at AS ModifiedAt
			FROM
				`user`
			WHERE
				login = @Login
				AND deleted = 0
			LIMIT 1
        ";
        return await _connection.QueryFirstOrDefaultAsync<UserRow>(sql, new
        {
	        Login = login.ToString()
        });
    }

    public async Task<int> InsertAsync(UserRow userRow)
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
			select LAST_INSERT_ID();
		";
	    return await _connection.ExecuteScalarAsync<int>(sql, userRow);
    }
}