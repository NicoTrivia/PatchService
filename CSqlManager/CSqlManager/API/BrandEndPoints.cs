
namespace CSqlManager;


public class BrandEndPoints: SecureEnpoint
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/brand", GetAll);
        app.MapGet("/brand/{code}", GetByCode);
        app.MapPost("/brand", Create);
        app.MapPut("/brand", Update);
        app.MapDelete("/brand/{code}", DeleteByCode);
    }
    
    public static IResult GetAll()
    {
        var access = new BrandAccess();
        var list = access.GetBrands();

        return Results.Ok(list);
    }

    public static IResult GetByCode(HttpContext context, string code)
    {
        var access = new BrandAccess();
        var brand = access.GetBrand(code);

        return Results.Ok(brand);
    }

    
    public static IResult Create(HttpContext context, Brand brand)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Debug($"BRAND POST {brand}");
       
        var access = new BrandAccess();
        access.Create(brand);
        MyLogManager.Debug($"Brand created : {brand.Code} - {brand.Name} by {claims.User} / {claims.Tenant}");
        return Results.Ok(brand);
    }
    public static IResult Update(HttpContext context, Brand brand)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Debug($"BRAND PUT {brand}");
        
        var access = new BrandAccess();
        access.Update(brand);
        MyLogManager.Debug($"Brand Updated : {brand.Code} - {brand.Name} by {claims.User} / {claims.Tenant}");
        return Results.Ok(brand);
    }
    public static IResult DeleteByCode(HttpContext context, string code)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }

        var access1 = new EcuAccess();
        var success1 = access1.DeleteByBrandCode(code);

        var access = new BrandAccess();
        var success = access.DeleteBrandByCode(code);

        MyLogManager.Debug($"Brand Deleted : {code} by {claims.User} / {claims.Tenant}");

        return Results.Ok(success);
    }
}