using WConnect.Auth.Core.DatabaseModels;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Core.Repositories;

public interface IUserRepository
{
    public Task<User?> FindUserByLoginAsync(Login login);
    public Task<int> InsertAsync(User user);
}