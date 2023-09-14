using WConnect.Auth.Core.Database.PersistenceModels;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Core.Database;

public interface IUserRepository
{
    public Task<UserRow?> FindUserByLoginAsync(Login login);
    public Task<int> InsertAsync(UserRow userRow);
}