namespace WConnect.Auth.Domain.ValueObjects;

public class Password
{
    private string _password;
    private const int MinLength = 5;
    private const int MaxLength = 100;

    public Password()
    {
        
    }
    
    public Password(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        if (password.Length is < MinLength or > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(password));
        }
        _password = password.Trim();
    }

    public override string ToString() => _password;
}