using System.Data;
using Npgsql;
namespace CSqlManager;

public class TenantAccess : DbAccess
{
    private void AddFromReader(NpgsqlDataReader reader, Tenant tenant)
    {
        tenant.code = getString(reader, "code");
        tenant.name = getString(reader, "name");
        tenant.email = getString(reader, "email");
        tenant.level = getString(reader, "level");
        tenant.active = reader.GetBoolean(reader.GetOrdinal("active"));
        tenant.creation_date = getDateTime(reader, "creation_date");
        tenant.expiration_date =  getDateTime(reader, "expiration_date");
    }
    
    public List<Tenant> GetTenants()
    {
        List<Tenant> requestResult = new List<Tenant>();
        
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT code,name,email,level,active,creation_date,expiration_date FROM ps_tenant ORDER BY code";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Tenant current = new Tenant();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
            Close(Connection);
        }
        
        return requestResult;
    }

    public Tenant GetTenantByCode(string code)
    {
        Tenant tenant = new Tenant();
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT code,name,email,level,active,creation_date,expiration_date FROM ps_tenant WHERE code = @Code";
            
            command.Parameters.AddWithValue("Code", code);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                AddFromReader(reader, tenant);
            }
            Close(Connection);
        }

        return tenant;
    }

    public int GetNextFileId(string code)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT next_file_id FROM ps_tenant WHERE code = @Code";
            
            command.Parameters.AddWithValue("Code", code);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                  int id = (int)getInt(reader, "next_file_id", true)!;
                  return id;
            }
            Close(Connection);
        }

        return 0;
    }



    public void Create(Tenant tenant)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"INSERT INTO ps_tenant (code, name, email, level, active, creation_date, expiration_date, next_file_id)" +
                                  $" VALUES (@code, @name, @email, @level, @active, @creation_date, @expiration_date, @next_file_id);";
            command.Parameters.AddWithValue("code", GetParam(tenant.code));
            command.Parameters.AddWithValue("name", GetParam(tenant.name));
            command.Parameters.AddWithValue("email", GetParam(tenant.email));
            command.Parameters.AddWithValue("level",GetParam(tenant.level));
            command.Parameters.AddWithValue("active",GetParam(tenant.active));
            command.Parameters.AddWithValue("creation_date", DateTime.Today);
            command.Parameters.AddWithValue("next_file_id", 1);

            command.Parameters.AddWithValue("expiration_date", GetParam(tenant.expiration_date));
            command.ExecuteNonQuery();
            Close(Connection);
        }
    }
    
    public void Update(Tenant tenant)
    {
        if (tenant == null) {
            return;
        }
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = 
                "UPDATE ps_tenant" +
                $" SET name = @name, email = @email, level = @level ,active = @active," +
                $"expiration_date = @expiration_date WHERE code = @code;";
            
            command.Parameters.AddWithValue("code", tenant.code!);
            
            command.Parameters.AddWithValue("name", GetParam(tenant.name));
            command.Parameters.AddWithValue("email", GetParam(tenant.email));
            command.Parameters.AddWithValue("level",GetParam(tenant.level));
            command.Parameters.AddWithValue("active",GetParam(tenant.active));
            command.Parameters.AddWithValue("expiration_date", GetParam(tenant.expiration_date));
            
            command.ExecuteNonQuery();
            Close(Connection);
        }
    }
    
    public void nextFileId(string tenant)
    {
        if (tenant == null) {
            return;
        }
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = 
                "UPDATE ps_tenant SET next_file_id = (next_file_id + 1) WHERE code = @code;";
            
            command.Parameters.AddWithValue("code", tenant);
            
            command.ExecuteNonQuery();
            Close(Connection);
        }
    }

    public Boolean DeleteTenantByCode(string code) {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"DELETE FROM ps_tenant WHERE code = @code";
            
            command.Parameters.AddWithValue("code", code);
            int count = command.ExecuteNonQuery();
            Close(Connection);
            return count > 0;
        }
    }
}