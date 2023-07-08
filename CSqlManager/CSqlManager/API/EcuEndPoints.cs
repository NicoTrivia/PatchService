
namespace CSqlManager;


public class EcuEndPoints: SecureEnpoint
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/ECU/{BrandCode}", GetByBrandCode);
        app.MapGet("/ECU/{BrandCode}/{Fuel}", GetByBrandCodeAndFuel);
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
}