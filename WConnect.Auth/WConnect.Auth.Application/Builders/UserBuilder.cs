using WConnect.Auth.Core.UseCases.SignIn;
using WConnect.Auth.Domain.Entities;
using WConnect.Auth.Domain.ValueObjects;

namespace WConnect.Auth.Application.Builders;

public class UserBuilder: IUserBuilder
{
    private string _name = null!;
    private Password _password = null!;
    private Login _login = null!;
    private Uri? _photoUri;
    private int? _id = null!;
    public IUserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public IUserBuilder WithPassword(string password)
    {
        _password = new(password);
        return this;
    }

    public IUserBuilder WithLogin(string login)
    {
        _login = new(login);
        return this;
    }

    public IUserBuilder WithPhotoUrl(string? uri)
    {
        if (uri is not null)
        {
            _photoUri = new(uri);
        }
        return this;
    }

    public IUserBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public User Build()
    {
        ArgumentNullException.ThrowIfNull(_login);
        ArgumentNullException.ThrowIfNull(_password);
        ArgumentNullException.ThrowIfNull(_name);
        Credential credential = _id is not null 
            ? new(_id ?? 0, _login, _password) 
            : new(_login, _password);
        PersonalData personalData = new(_name, _photoUri);
        return new(credential, personalData);
    }
}