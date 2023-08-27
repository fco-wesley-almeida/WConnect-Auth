namespace WConnect.Auth.Domain.ValueObjects;

public class Credential
{
    public Credential(Login login, Password password)
    {
        Login = login;
        Password = password;
    }

    public Login Login { get; }
    public Password Password { get; }
}