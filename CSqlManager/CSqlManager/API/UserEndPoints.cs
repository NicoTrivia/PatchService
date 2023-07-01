namespace CSqlManager;

public class UserEndPoints
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/user", GetAll);
        app.MapGet("/user/tenant/{Tenant}", GetByTenant);
        app.MapGet("/user/{Id}", GetById);
        app.MapGet("/user/login/{Tenant}/{Login}/{Password}", UserLogin);
        
        app.MapPost("/user", Create);
        
        app.MapPut("/user", Update);
    }
    
    public static IResult GetAll()
    {
        var access = new UserAccess();
        var list = access.GetUsers();

        return Results.Ok(list);
    }

    public static IResult GetByTenant(string Tenant)
    {
        var access = new UserAccess();
        var list = access.GetUsersByTenant(Tenant);

        return Results.Ok(list);
    }
    
    public static IResult GetById(int Id)
    {
        var access = new UserAccess();
        var user = access.GetUserById(Id);

        return Results.Ok(user);
    }

    public static IResult Create(User user)
    {
        Console.WriteLine($"USER POST {user}");
        
        var access = new UserAccess();
        access.Create(user);

        return Results.Ok(user);
    }
    public static IResult Update(User user)
    {
        Console.WriteLine($"USER PUT {user}");
       
        var access = new UserAccess();
        access.Update(user);

        return Results.Ok(user);
    }

    public static IResult UserLogin(string Tenant, string Login, string Password)
    {
        Console.WriteLine($"Trying to login in tenant : : {Tenant} as : {Login} with password : {Password} ");
        
        UserAccess access = new UserAccess();
        TenantAccess tenantAccess = new TenantAccess();
        User user = access.Login(Tenant,Login,Password);

        string level = tenantAccess.GetTenantByCode(user.tenant).level;
        user.jwt = User.GenerateJwtToken(user.login, user.tenant, level);
        
        return Results.Ok(user);
    }
}