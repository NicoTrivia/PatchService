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
            command.CommandText = $"SELECT code,name FROM ps_brand";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                requestResult.Add(new Brand(reader.GetString(0), reader.GetString(1)));
                
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
                return new Brand(code, reader.GetString(0));
            }

        }
        return null;
    }
}