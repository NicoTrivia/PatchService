using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
namespace CSqlManager;

public class BrandAccess : DbAccess
{
    public List<Brand> GetBrands()
    {
        List<Brand> requestResult = new List<Brand>();
        
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT code,name FROM ps_brand ORDER BY name";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                requestResult.Add(new Brand(reader.GetString(reader.GetOrdinal("code")), reader.GetString(reader.GetOrdinal("name"))));
                
            }

        }
        
        return requestResult;
    }
    
    public Brand? GetBrand(string code)
    {
        
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT name FROM ps_brand WHERE code = @Code";
            if (command.Parameters.Contains("Code"))
                command.Parameters.Remove("Code");

            command.Parameters.AddWithValue("Code", code);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Brand(code, reader.GetString(reader.GetOrdinal("name")));
            }

        }
        return null;
    }
}