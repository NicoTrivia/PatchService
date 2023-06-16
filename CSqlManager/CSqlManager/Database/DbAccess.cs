using System.Data;
using Npgsql;

namespace CSqlManager;

public abstract class DbAccess
{
    static readonly string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";

    public NpgsqlConnection? Connection = null;

    public NpgsqlConnection GetConnection()
    {
        if (Connection == null || Connection.State != ConnectionState.Open)
        {
            Connection = new NpgsqlConnection(connString);
            Connection.Open();
        }
        return Connection;
    }

    public NpgsqlCommand CreateCommand()
    {
        var connection = GetConnection();
        return connection.CreateCommand();
    }

    public void Close()
    {
        if(Connection != null)
            Connection.Close();
        Connection = null;
    }
}