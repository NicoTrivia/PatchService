namespace CSqlManager;

public class TenantEndPoints
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/Tenant", GetAll);
        app.MapGet("/Tenant/{Code}", GetByCode);
        //app.MapPost("/Tenant/");
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
    
    //public static
}