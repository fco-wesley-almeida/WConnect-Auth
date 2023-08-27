using WConnect.Auth.Domain.Entities;

namespace WConnect.Auth.Core.DatabaseModels;

public class User
{
    public int Id {get; private set;}
    public string Name {get; private set;}
    public string Login {get; private set;}
    public string Password {get; private set;}
    public string? PhotoUrl {get; private set;}
    public DateTime CreatedAt {get; private set;}
    public DateTime ModifiedAt {get; private set;}
    public bool Deleted {get; private set;}

    public User()
    {
        
    }
    
    private User(int id, string name, string login, string password, string? photoUrl, DateTime createdAt,
        DateTime modifiedAt, bool deleted)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Login = login ?? throw new ArgumentNullException(nameof(login));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        PhotoUrl = photoUrl;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
        Deleted = deleted;
    }

    public User(UserDomain userDomain)
    {
        Id = 0;
        Name = userDomain.PersonalData.Name;
        Login = userDomain.Credential.Login.ToString();
        Password = userDomain.Credential.Password.ToString();
        PhotoUrl = userDomain.PersonalData.PhotoUrl?.ToString();
        CreatedAt = DateTime.Now;
        ModifiedAt = DateTime.Now;
        Deleted = false;
    }
}