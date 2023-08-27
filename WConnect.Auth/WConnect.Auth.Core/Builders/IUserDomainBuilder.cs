using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Core.Builders;

public interface IUserDomainBuilder
{
    public IUserDomainBuilder WithName(string name);
    public IUserDomainBuilder WithPassword(string password);
    public IUserDomainBuilder WithLogin(string login);
    public IUserDomainBuilder WithPhotoUrl(string? uri);
    public UserDomain Build();
}