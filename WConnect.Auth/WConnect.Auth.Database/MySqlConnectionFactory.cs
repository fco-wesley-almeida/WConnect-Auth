using System.Data;
using MySqlConnector;

namespace WConnect.Auth.Database;

public class MySqlConnectionFactory
{
    public static IDbConnection Create()
    {  
         return new MySqlConnection(ConnectionString());
    }

    private static string ConnectionString()
    {
        var envVar = Environment.GetEnvironmentVariable("DB_CONNECTION");
        ArgumentException.ThrowIfNullOrEmpty(envVar);
        return envVar;
    }
}