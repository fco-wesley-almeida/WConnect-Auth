namespace WConnect.Auth.Domain.ValueObjects;

public class Password
{
    private string _password;

    public Password(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        if (password.Length is < 5 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(password));
        }
        _password = password;
    }

    public override string ToString() => _password;
}