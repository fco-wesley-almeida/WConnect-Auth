namespace WConnect.Auth.Domain.ValueObjects;

public class Credential
{
    private int? _id;
    public int Id => _id ?? throw new InvalidOperationException("This user has not any id associated.");
    public Login Login { get; }
    public Password Password { get; }
    
    public Credential(Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        Login = login;
        Password = password;
        _id = null;
    }
    
    public Credential(int id, Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        _id = id;
        Login = login;
        Password = password;
    }
    
    public Credential() {}


}