using WConnect.Auth.Core.Builders;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Application.Builders;

public class UserDomainBuilder: IUserDomainBuilder
{
    private string _name = null!;
    private Password _password = null!;
    private Login _login = null!;
    private Uri? _photoUri;
    public IUserDomainBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public IUserDomainBuilder WithPassword(string password)
    {
        _password = new(password);
        return this;
    }

    public IUserDomainBuilder WithLogin(string login)
    {
        _login = new(login);
        return this;
    }

    public IUserDomainBuilder WithPhotoUrl(string? uri)
    {
        if (uri is not null)
        {
            _photoUri = new(uri);
        }
        return this;
    }

    public UserDomain Build()
    {
        ArgumentNullException.ThrowIfNull(_login);
        ArgumentNullException.ThrowIfNull(_password);
        ArgumentNullException.ThrowIfNull(_name);
        ArgumentNullException.ThrowIfNull(_login);
        Credential credential = new(_login, _password);
        PersonalData personalData = new(_name, _photoUri);
        return new(credential, personalData);
    }
}