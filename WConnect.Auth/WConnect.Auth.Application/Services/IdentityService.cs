using WConnect.Auth.Core.Services;
using System.Text;

namespace WConnect.Auth.Application.Services;

/*
 * Absolutely, this is not a secure implementation of authentication with token.
 * But, the idea of the project is to provide a simple authentication way, in minor scale.
 * In other words, this implementation CAN NOT be used in production contexts.
 */
public class IdentityService: IIdentityService
{
    public string Encrypt(int userId)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(userId.ToString());
        return Convert.ToBase64String(textBytes);
    }

    public int Decrypt(string accessToken)
    {
        try
        {
            byte[] base64Bytes = Convert.FromBase64String(accessToken);
            string decodedText = Encoding.UTF8.GetString(base64Bytes);
            return Convert.ToInt32(decodedText);
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Access token is not valid.");
        }
    }
}