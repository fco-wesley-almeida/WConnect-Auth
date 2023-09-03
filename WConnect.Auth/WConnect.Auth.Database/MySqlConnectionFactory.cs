using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace WConnect.Auth.Database;

public class MySqlConnectionFactory
{
    public static IDbConnection Create(IConfiguration configuration)
    {  
         return new MySqlConnection(ConnectionString(configuration));
    }

    private static string ConnectionString(IConfiguration configuration)
    {
        return configuration.GetConnectionString("Default") 
               ?? throw new Exception("The Default connection string is not connected. ");
    }
}