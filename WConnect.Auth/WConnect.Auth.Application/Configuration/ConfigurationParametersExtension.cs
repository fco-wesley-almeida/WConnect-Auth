using WConnect.Auth.Application.Exceptions;

namespace WConnect.Auth.Application.Configuration;

public static class ConfigurationParametersExtension
{
    public static string GetString(this IConfiguration configuration, string paramName)
    {
        string? value = configuration[paramName];
        if (string.IsNullOrEmpty(value))
        {
            throw new MissingConfigurationParameterException(paramName);
        }
        return value;
    }
    
    public static int GetInt(this IConfiguration configuration, string paramName)
    {
        string? value = configuration[paramName];
        if (string.IsNullOrEmpty(value))
        {
            throw new MissingConfigurationParameterException(paramName);
        }
        return Convert.ToInt32(value);
    }
}