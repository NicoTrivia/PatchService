using System.Data;
using Npgsql;
namespace CSqlManager;

public class TenantAccess : DbAccess
{
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
                current.code = reader.GetString(reader.GetOrdinal("code"));
                current.name = reader.GetString(reader.GetOrdinal("name"));
                current.email = reader.GetString(reader.GetOrdinal("email"));
                current.level = reader.GetString(reader.GetOrdinal("level"));
                current.active = reader.GetBoolean(reader.GetOrdinal("active"));
                current.creation_date = reader.GetDateTime(reader.GetOrdinal("creation_date"));
                current.expiration_date = reader.GetDateTime(reader.GetOrdinal("expiration_date"));
                
                
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
                current.code = reader.GetString(reader.GetOrdinal("code"));
                current.name = reader.GetString(reader.GetOrdinal("name"));
                current.email = reader.GetString(reader.GetOrdinal("email"));
                current.level = reader.GetString(reader.GetOrdinal("level"));
                current.active = reader.GetBoolean(reader.GetOrdinal("active"));
                current.creation_date = reader.GetDateTime(reader.GetOrdinal("creation_date"));
                current.expiration_date = reader.GetDateTime(reader.GetOrdinal("expiration_date"));
                
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
            command.Parameters.AddWithValue("code", getParamStr(tenant.code));
            command.Parameters.AddWithValue("name", getParamStr(tenant.name));
            command.Parameters.AddWithValue("email", getParamStr(tenant.email));
            command.Parameters.AddWithValue("level",getParamStr(tenant.level));
            command.Parameters.AddWithValue("active",getParamStr(tenant.active));
            command.Parameters.AddWithValue("creation_date", new DateTime());
            command.Parameters.AddWithValue("expiration_date", getParamStr(tenant.expiration_date));
            command.ExecuteNonQuery();
        }
    }

    public object getParamStr(object? param){
        return param == null ? DBNull.Value : param;
    }
    
    public void Update(Tenant tenant)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = 
                "UPDATE ps_tenant" +
                $"SET name = '{tenant.name}', email = '{tenant.email}', level = '{tenant.level}',active = {tenant.active.ToString().ToLower()}," +
                $"expiration_date = 'tenant.expiration_date' WHERE code = @Code;";
            command.Parameters.AddWithValue("Code", tenant.code);
            command.ExecuteNonQuery();
        }
        
    }
    
}