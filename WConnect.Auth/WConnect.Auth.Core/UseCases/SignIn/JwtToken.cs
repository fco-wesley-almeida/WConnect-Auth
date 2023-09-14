namespace WConnect.Auth.Core.UseCases.SignIn;

public class JwtToken
{
    public JwtToken(string accessToken, DateTime accessTokenExpiryTime)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        AccessToken = accessToken;
        AccessTokenExpiryTime = accessTokenExpiryTime;
    }
    public string AccessToken { get; }
    public DateTime AccessTokenExpiryTime { get; }
}