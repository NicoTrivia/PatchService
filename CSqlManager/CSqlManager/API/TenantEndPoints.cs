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
        app.MapDelete("/tenant/{code}", DeleteByCode);
    }
    
    public static IResult GetAll()
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

        return Results.Ok(tenant);
    }
    public static IResult Update(Tenant tenant)
    {
        Console.WriteLine($"TENANT PUT {tenant}");
        
        var access = new TenantAccess();
        access.Update(tenant);

        return Results.Ok(tenant);
    }
    public static IResult DeleteByCode(string code)
    {
        var access1 = new UserAccess();
        var success1 = access1.DeleteUserByTenant(code);

        var access = new TenantAccess();
        var success = access.DeleteTenantByCode(code);

        return Results.Ok(success);
    }
}

