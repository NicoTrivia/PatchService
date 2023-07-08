
namespace CSqlManager;


public class BrandEndPoints: SecureEnpoint
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/brand", GetAll);
    }
    
    
    public static IResult GetAll()
    {
        var access = new BrandAccess();
        var list = access.GetBrands();

        return Results.Ok(list);
    }
}