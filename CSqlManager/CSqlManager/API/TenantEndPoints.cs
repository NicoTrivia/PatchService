using System.Text.Json;

namespace CSqlManager;

public class TenantEndPoints: SecureEnpoint
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/tenant", GetAll);
        app.MapGet("/tenant/{Code}", GetByCode);
        app.MapGet("/next_file_id", GetNextFileId);
        app.MapPost("/tenant", Create);
        
        app.MapPut("/tenant", Update);
        app.MapDelete("/tenant/{code}", DeleteByCode);
    }
    
    public static IResult GetAll(HttpContext context)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TenantAccess();
        var list = access.GetTenants();

        return Results.Ok(list);
    }

    public static IResult GetByCode(HttpContext context, string Code)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR" && claims.Tenant != Code)) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TenantAccess();
        var tenant = access.GetTenantByCode(Code);

        return Results.Ok(tenant);
    }

    public static IResult GetNextFileId(HttpContext context)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TenantAccess();
        var nextId = access.GetNextFileId(claims.Tenant);

        return Results.Ok(nextId);
    }

    public static IResult Create(HttpContext context, Tenant tenant)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Log($"TENANT POST {tenant}");
       
        var access = new TenantAccess();
        access.Create(tenant);

        return Results.Ok(tenant);
    }
    public static IResult Update(HttpContext context, Tenant tenant)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Log($"TENANT PUT {tenant}");
        
        var access = new TenantAccess();
        access.Update(tenant);

        return Results.Ok(tenant);
    }
    public static IResult DeleteByCode(HttpContext context, string code)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access1 = new UserAccess();
        var success1 = access1.DeleteUserByTenant(code);

        var access = new TenantAccess();
        var success = access.DeleteTenantByCode(code);

        return Results.Ok(success);
    }
}

