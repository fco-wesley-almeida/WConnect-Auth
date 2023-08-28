using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.Builders;

public interface IUserBuilder
{
    public IUserBuilder WithName(string name);
    public IUserBuilder WithPassword(string password);
    public IUserBuilder WithLogin(string login);
    public IUserBuilder WithPhotoUrl(string? uri);
    public User Build();
}