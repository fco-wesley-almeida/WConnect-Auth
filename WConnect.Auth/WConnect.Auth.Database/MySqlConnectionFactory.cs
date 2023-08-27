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
        return "Server=localhost;User ID=m01;Password=m01;Database=wconnect-auth";
    }
}