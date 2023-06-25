using System.Data;
using Npgsql;

namespace CSqlManager;

public class DbAccess
{
    static readonly string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";

    private NpgsqlConnection? Connection = null;
    protected object GetParam(object? param) => param == null ? DBNull.Value : param;
    private NpgsqlConnection GetConnection()
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
    
    public void ResetDatabase()
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = "DELETE FROM ps_ecu";
            command.ExecuteNonQuery();
               
            command.CommandText = "DELETE FROM ps_brand";
            command.ExecuteNonQuery();
            Console.WriteLine("Database content deleted");
        }
    
    }
    public void RemoveColInBrand(string name)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"ALTER TABLE ps_brand DROP COLUMN {name}";
            command.ExecuteNonQuery();
            Console.WriteLine($"Column {name} deleted from brands");
        }
    }
    
    public void RemoveColInECU(string name)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"ALTER TABLE ps_ecu DROP COLUMN {name}";
            command.ExecuteNonQuery();
            Console.WriteLine($"Column {name} deleted from ecus");
        }
    }

    public void CreateTable(string name, List<(string name, string type, string typePrecisions ,bool isPrimary)> content)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"CREATE TABLE {name} (";
            foreach (var information in content)
            {
                string primary =  information.isPrimary ? "PRIMARY KEY" : "";
                command.CommandText += $"{information.name} {information.type} ({information.typePrecisions}) {primary},";
            }

            command.CommandText += ")";
            command.ExecuteNonQuery();
            Console.WriteLine($"Table {name} created");
        }
    }


    public string? getString(NpgsqlDataReader reader, string code) {
        if (!reader.IsDBNull(reader.GetOrdinal(code)))
            return reader.GetString(reader.GetOrdinal(code));
        return null;
    }
    public DateTime? getDateTime(NpgsqlDataReader reader, string code) {
        if (!reader.IsDBNull(reader.GetOrdinal(code)))
            return reader.GetDateTime(reader.GetOrdinal(code));
        return null;
    }
}