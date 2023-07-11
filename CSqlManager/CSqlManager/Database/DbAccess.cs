using System.Data;
using Npgsql;

namespace CSqlManager;

public class DbAccess
{
    public static readonly string connString = "Server=" + Variables.RetrieveVariable("Server") +
                                        "Port=" + Variables.RetrieveVariable("Port") +
                                        "Database=" + Variables.RetrieveVariable("Database") +
                                        "User Id=" + Variables.RetrieveVariable("User Id") +
                                        "Password=" + Variables.RetrieveVariable("Password") +
                                        "MaxPoolSize=" + Variables.RetrieveVariable("MaxPoolSize");

    private NpgsqlConnection? Connection = null;
    protected object GetParam(object? param) => param == null ? DBNull.Value : param;
    private NpgsqlConnection GetConnection()
    {
        if (Connection == null || Connection.State != ConnectionState.Open)
        {
            MyLogManager.Log("Npgsq OPEN new connecion since former state is "+(Connection == null ? "null": Connection.State.ToString()));
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
            MyLogManager.Log("Database content deleted");
        }
    
    }
    public void RemoveColInBrand(string name)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"ALTER TABLE ps_brand DROP COLUMN {name}";
            command.ExecuteNonQuery();
            MyLogManager.Log($"Column {name} deleted from brands");
        }
    }
    
    public void RemoveColInECU(string name)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"ALTER TABLE ps_ecu DROP COLUMN {name}";
            command.ExecuteNonQuery();
            MyLogManager.Log($"Column {name} deleted from ecus");
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
            MyLogManager.Log($"Table {name} created");
        }
    }

    public bool? getBoolean(NpgsqlDataReader reader, string code, bool? defaultValue = false) {
        if (!reader.IsDBNull(reader.GetOrdinal(code)))
            return reader.GetBoolean(reader.GetOrdinal(code));
        if (defaultValue != null)
            return defaultValue;
        return null;
    }

    public int? getInt(NpgsqlDataReader reader, string code, Boolean notNull = false) {
        if (!reader.IsDBNull(reader.GetOrdinal(code)))
            return reader.GetInt32(reader.GetOrdinal(code));
        if (notNull)
            return 0;
        return null;
    }


    public string? getString(NpgsqlDataReader reader, string code, Boolean notNull = false) {
        if (!reader.IsDBNull(reader.GetOrdinal(code)))
            return reader.GetString(reader.GetOrdinal(code));
        if (notNull)
            return "";
        return null;
    }

    public DateTime? getDateTime(NpgsqlDataReader reader, string code, Boolean notNull = false) {
        if (!reader.IsDBNull(reader.GetOrdinal(code)))
            return reader.GetDateTime(reader.GetOrdinal(code));
        if (notNull)
            return new DateTime();
        return null;
    }
}