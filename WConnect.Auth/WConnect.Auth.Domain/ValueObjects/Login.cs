namespace WConnect.Auth.Domain.ValueObjects;

public class Login
{
    private string _login;
    private const int MinLength = 5;
    private const int MaxLength = 20;

    public Login()
    {
        
    }
    
    public Login(string login)
    {
        ArgumentException.ThrowIfNullOrEmpty(login);
        _login = login.Length switch
        {
            < MinLength => throw new ArgumentOutOfRangeException(nameof(login),
                $"The login must to have at least {MinLength} length."),
            > MaxLength => throw new ArgumentOutOfRangeException(nameof(login),
                $"The login must to have at max {MaxLength} length."),
            _ => login.Trim()
        };
    }
    public override string ToString() => _login;
}