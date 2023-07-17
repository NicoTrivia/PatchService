using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
namespace CSqlManager;

public class BrandAccess : DbAccess
{
    public List<Brand> GetBrands()
    {
        List<Brand> requestResult = new List<Brand>();
        
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT code,name FROM ps_brand ORDER BY name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                requestResult.Add(new Brand(reader.GetString(reader.GetOrdinal("code")), reader.GetString(reader.GetOrdinal("name"))));
            }
            Close(Connection);
        }
        
        return requestResult;
    }
    
    public Brand? GetBrand(string code)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);

            command.CommandText = $"SELECT name FROM ps_brand WHERE code = @Code";
            if (command.Parameters.Contains("Code"))
                command.Parameters.Remove("Code");

            command.Parameters.AddWithValue("Code", code);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Brand(code, reader.GetString(reader.GetOrdinal("name")));
            }
            Close(Connection);
        }
        return null;
    }

    
    public void Create(Brand brand)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);

            command.CommandText = $"INSERT INTO ps_brand (code, name) VALUES (@code, @name)";
            command.Parameters.AddWithValue("code", GetParam(brand.Code));
            command.Parameters.AddWithValue("name", GetParam(brand.Name));

            command.ExecuteNonQuery();
            Close(Connection);
        }
    }
    
    public void Update(Brand brand)
    {
        if (brand == null) {
            return;
        }
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = "UPDATE ps_brand SET name = @name WHERE code = @code";
            
            command.Parameters.AddWithValue("code", brand.Code!);
            command.Parameters.AddWithValue("name", GetParam(brand.Name)); 
            command.ExecuteNonQuery();
            Close(Connection);
        }
    }

    public Boolean DeleteBrandByCode(string code) {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"DELETE FROM ps_brand WHERE code = @code";
            
            command.Parameters.AddWithValue("code", code);
            int count = command.ExecuteNonQuery();
            Close(Connection);
            return count > 0;
        }
    }
}