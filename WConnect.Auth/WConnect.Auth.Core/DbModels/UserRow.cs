using WConnect.Auth.Core.Providers;
using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.DbModels;

public class UserRow
{
    public int Id {get; private set;}
    public string Name {get; private set;}
    public string Login {get; private set;}
    public string Password {get; private set;}
    public string? PhotoUrl {get; private set;}
    public DateTime CreatedAt {get; private set;}
    public DateTime ModifiedAt {get; private set;}
    public bool Deleted {get; private set;}

    public UserRow()
    {
        
    }

    public UserRow(User user, ITimeProvider timeProvider)
    {
        Id = 0;
        Name = user.PersonalData.Name;
        Login = user.Credential.Login.ToString();
        Password = user.Credential.Password.ToString();
        PhotoUrl = user.PersonalData.PhotoUrl?.ToString();
        CreatedAt = timeProvider.Now();
        ModifiedAt = timeProvider.Now();
        Deleted = false;
    }
}