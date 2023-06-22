using System.Data;
using Npgsql;
namespace CSqlManager;

public class TenantAccess : DbAccess
{
    private void AddFromReader(NpgsqlDataReader reader, Tenant tenant)
    {
        tenant.code = reader.GetString(reader.GetOrdinal("code"));
        tenant.name = reader.GetString(reader.GetOrdinal("name"));
        tenant.email = reader.GetString(reader.GetOrdinal("email"));
        tenant.level = reader.GetString(reader.GetOrdinal("level"));
        tenant.active = reader.GetBoolean(reader.GetOrdinal("active"));
        tenant.creation_date = reader.GetDateTime(reader.GetOrdinal("creation_date"));
        tenant.expiration_date = reader.GetDateTime(reader.GetOrdinal("expiration_date"));
    }
    
    
    public List<Tenant> GetTenants()
    {
        List<Tenant> requestResult = new List<Tenant>();
        
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT code,name,email,level,active,creation_date,expiration_date FROM ps_tenant";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Tenant current = new Tenant();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }

        }
        
        return requestResult;
    }

    public Tenant GetTenantByCode(string code)
    {
        Tenant tenant = new Tenant();
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT code,name,email,level,active,creation_date,expiration_date FROM ps_tenant WHERE code = @Code";
            
            command.Parameters.AddWithValue("Code", code);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                Tenant current = new Tenant();
                AddFromReader(reader, current);
            }
        }

        return tenant;
    }

    public void Create(Tenant tenant)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"INSERT INTO ps_tenant (code, name, email, level, active, creation_date, expiration_date)" +
                                  $"VALUES (@code, @name, @email, @level, @active, @creation_date, @expiration_date);";
            command.Parameters.AddWithValue("code", GetParam(tenant.code));
            command.Parameters.AddWithValue("name", GetParam(tenant.name));
            command.Parameters.AddWithValue("email", GetParam(tenant.email));
            command.Parameters.AddWithValue("level",GetParam(tenant.level));
            command.Parameters.AddWithValue("active",GetParam(tenant.active));
            command.Parameters.AddWithValue("creation_date", new DateTime());
            command.Parameters.AddWithValue("expiration_date", GetParam(tenant.expiration_date));
            command.ExecuteNonQuery();
        }
    }
    
    public void Update(Tenant tenant)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = 
                "UPDATE ps_tenant" +
                $"SET name = @name, email = @email, level = @level ,active = @active," +
                $"expiration_date = @expiration_date WHERE code = @code;";
            
            command.Parameters.AddWithValue("code", tenant.code);
            
            command.Parameters.AddWithValue("name", GetParam(tenant.name));
            command.Parameters.AddWithValue("email", GetParam(tenant.email));
            command.Parameters.AddWithValue("level",GetParam(tenant.level));
            command.Parameters.AddWithValue("active",GetParam(tenant.active));
            command.Parameters.AddWithValue("expiration_date", GetParam(tenant.expiration_date));
            
            command.ExecuteNonQuery();
        }
        
    }
    
}