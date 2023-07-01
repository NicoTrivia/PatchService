namespace CSqlManager;

public class UserEndPoints
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/user", GetAll);
        app.MapGet("/user/tenant/{Tenant}", GetByTenant);
        app.MapGet("/user/{Id}", GetById);
        app.MapPost("/authenticate", UserLogin);
        
        app.MapPost("/user", Create);
        
        app.MapPut("/user", Update);
        app.MapPut("/password", Update);

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
        Console.WriteLine($"USER POST {user.id} - {user.login} - {user.firstname}- {user.lastname}");
        
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

    
    public static IResult UpdatePassword(User user)
    {
        Console.WriteLine($"USER PASSWORD: {user.id}");
       
        var access = new UserAccess();
        access.UpdatePassword(user);

        return Results.Ok(user);
    }


    public static IResult UserLogin(HttpContext context)
    {
        Console.WriteLine("Authenticate :");
        var Tenant = "";
        var Login = "";
        var Password = "";
        foreach (var formPart in context.Request.Form) {
            if (formPart.Key == "tenant") {
               Tenant = formPart.Value;
            }
            else if (formPart.Key == "login") {
               Login = formPart.Value;
            }
            else if (formPart.Key == "password") {
            Password = formPart.Value;
            }
        }

        Console.WriteLine($"Trying to login in tenant : : {Tenant} as : {Login} with password : ******** ");
        
        UserAccess access = new UserAccess();
        TenantAccess tenantAccess = new TenantAccess();
        User? user = access.Login(Tenant,Login,Password);

        string level = tenantAccess.GetTenantByCode(user.tenant).level;
        user.jwt = User.GenerateJwtToken(user.login, user.tenant, level);
        
        return Results.Ok(user);
    }
}