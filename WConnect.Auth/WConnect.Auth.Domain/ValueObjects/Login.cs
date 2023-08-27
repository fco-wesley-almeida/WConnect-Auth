namespace WConnect.Auth.Domain.ValueObjects;

public class Login
{
    private string _login;
    private const int MinLength = 5;
    private const int MaxLength = 10;

    public Login(string login)
    {
        _login = login.Length switch
        {
            < MinLength => throw new ArgumentOutOfRangeException(nameof(login),
                $"The login must to have at least {MinLength} length."),
            > MaxLength => throw new ArgumentOutOfRangeException(nameof(login),
                $"The login must to have at max {MaxLength} length."),
            _ => login
        };
    }

    public override string ToString() => _login;
}