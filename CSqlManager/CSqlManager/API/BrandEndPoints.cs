
namespace CSqlManager;


public class BrandEndPoints
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/brand", Get);
    }
    
    
    public static IResult Get()
    {
        var access = new BrandAccess();
        var list = access.GetBrands();

        return Results.Ok(list);
    }
}