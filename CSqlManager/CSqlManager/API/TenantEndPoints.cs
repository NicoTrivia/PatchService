using System.Text.Json;

namespace CSqlManager;

public class TenantEndPoints
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/tenant", GetAll);
        app.MapGet("/tenant/{Code}", GetByCode);
        
        app.MapPost("/tenant", Create);
        
        app.MapPut("/tenant", Update);
    }
    
    public static IResult GetAll(string BrandCode)
    {
        var access = new TenantAccess();
        var list = access.GetTenants();

        return Results.Ok(list);
    }

    public static IResult GetByCode(string Code)
    {
        var access = new TenantAccess();
        var tenant = access.GetTenantByCode(Code);

        return Results.Ok(tenant);
    }

    public static IResult Create(Tenant tenant)
    {
        Console.WriteLine($"TENANT POST {tenant}");
       
        var access = new TenantAccess();
        access.Create(tenant);

        return Results.Ok();
    }
    public static IResult Update(Tenant tenant)
    {
        Console.WriteLine($"TENANT PUT {tenant}");
        
        var access = new TenantAccess();
        access.Update(tenant);

        return Results.Ok();
    }
/*
    public static async Task<string> getStringFromBody(HttpContext context) {
        using (StreamReader reader = new StreamReader(context.Request.Body, System.Text.Encoding.UTF8))
        {
            //string queryKey = context.Request.RouteValues["queryKey"].ToString();
            string jsonstring = await reader.ReadToEndAsync();
        
            return jsonstring;
        }
    }

    public static Tenant MapJsonToObject(string jsonString) {
            try
            {
                // Deserialize JSON string to object
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var obj = JsonSerializer.Deserialize<Tenant>(jsonString, options);
                 Console.WriteLine($"TENANT2 {obj}");
                return obj;
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization error
                Console.WriteLine("Invalid JSON data: " + ex.Message);
            }
            return null;
    }*/
}

