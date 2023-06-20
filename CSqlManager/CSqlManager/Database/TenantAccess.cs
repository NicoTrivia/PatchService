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
                current.Level = reader.GetString(reader.GetOrdinal("level"));
                current.active = reader.GetBoolean(reader.GetOrdinal("active"));
                current.creation_date = reader.GetString(reader.GetOrdinal("creation_date"));
                current.expiration_date = reader.GetString(reader.GetOrdinal("expiration_date"));
                
                
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
                current.Level = reader.GetString(reader.GetOrdinal("level"));
                current.active = reader.GetBoolean(reader.GetOrdinal("active"));
                current.creation_date = reader.GetString(reader.GetOrdinal("creation_date"));
                current.expiration_date = reader.GetString(reader.GetOrdinal("expiration_date"));
                
            }
        }

        return tenant;
    }

    public void PostTenant(Tenant tenant)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"INSERT INTO ps_tenant (code, name, email, level, active, creation_date, expiration_date)" +
                                  $"VALUES ('{tenant.code}', '{tenant.name}', '{tenant.email}', '{tenant.Level}'," +
                                  $" {tenant.active.ToString().ToLower()}, '{tenant.creation_date}', '{tenant.expiration_date}');";
            command.ExecuteNonQuery();
        }
    }

    public void PutTenant(Tenant tenant)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = 
                "UPDATE ps_tenant" +
                $"SET name = '{tenant.name}', email = '{tenant.email}', level = '{tenant.Level}',active = {tenant.active.ToString().ToLower()}," +
                $"creation_date = '{tenant.creation_date}', expiration_date = 'tenant.expiration_date' WHERE code = @Code;";
            command.Parameters.AddWithValue("Code", tenant.code);
            command.ExecuteNonQuery();
        }
        
    }
    
}