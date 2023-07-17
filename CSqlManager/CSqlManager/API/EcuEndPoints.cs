
namespace CSqlManager;


public class EcuEndPoints: SecureEnpoint
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/ECU/{BrandCode}", GetByBrandCode);
        app.MapGet("/ECU/{BrandCode}/{Fuel}", GetByBrandCodeAndFuel);
        app.MapPost("/ECU", Create);
        app.MapPut("/ECU", Update);
        app.MapDelete("/ECU/{brand_code}/{code}", DeleteByCode);
    }
    
    
    public static IResult GetByBrandCode(string BrandCode)
    {
        var access = new EcuAccess();
        var list = access.GetEcuByBrandCode(BrandCode, null);

        return Results.Ok(list);
    }

    public static IResult GetByBrandCodeAndFuel(string BrandCode, string Fuel)
    {
        var access = new EcuAccess();
        var list = access.GetEcuByBrandCode(BrandCode, Fuel);

        return Results.Ok(list);
    }

    public static IResult Create(HttpContext context, ECU ecu)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Log($"ECU POST {ecu}");
       
        var access = new EcuAccess();
        access.Create(ecu);
        MyLogManager.Log($"ECU created : {ecu.Brand_code} - {ecu.code} by {claims.User} / {claims.Tenant}");
        return Results.Ok(ecu);
    }
    public static IResult Update(HttpContext context, ECU ecu)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Log($"ECU PUT {ecu}");
        
        var access = new EcuAccess();
        access.Update(ecu);
        MyLogManager.Log($"ECU updated : {ecu.Brand_code} - {ecu.code} by {claims.User} / {claims.Tenant}");

        return Results.Ok(ecu);
    }
    public static IResult DeleteByCode(HttpContext context, string brand_code, string code)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }

        var access = new EcuAccess();
        var success = access.DeleteBrandByCode(brand_code, code);

        MyLogManager.Log($"ECU deleted : {brand_code} - {code} by {claims.User} / {claims.Tenant}");

        return Results.Ok(success);
    }
}