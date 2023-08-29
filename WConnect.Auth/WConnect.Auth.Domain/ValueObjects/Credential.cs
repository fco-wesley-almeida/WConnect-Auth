namespace WConnect.Auth.Domain.ValueObjects;

public class Credential
{
    public int? Id { get; }
    public Login Login { get; }
    public Password Password { get; }
    
    public Credential(Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        Login = login;
        Password = password;
        Id = null;
    }
    
    public Credential(int id, Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }
        Id = id;
        Login = login;
        Password = password;
    }
    
    public Credential() {}
}