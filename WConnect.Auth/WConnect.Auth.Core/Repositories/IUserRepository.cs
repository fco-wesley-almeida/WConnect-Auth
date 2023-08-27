using WConnect.Auth.Core.DbModels;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Core.Repositories;

public interface IUserRepository
{
    public Task<UserRow?> FindUserByLoginAsync(Login login);
    public Task<int> InsertAsync(UserRow userRow);
}