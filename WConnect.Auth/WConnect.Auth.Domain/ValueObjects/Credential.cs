namespace WConnect.Auth.Domain.ValueObjects;

public class Credential
{
    public Credential(Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        Login = login;
        Password = password;
    }
    
    public Credential() {}

    public Login Login { get; }
    public Password Password { get; }
}