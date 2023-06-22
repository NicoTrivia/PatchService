namespace CSqlManager;

public class TicketEndPoints
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/ticket", GetAll);
        app.MapGet("/ticket/{Tenant}", GetByTenant);

        app.MapPost("/ticket", Create);
        
        app.MapPut("/ticket", Update);
    }
    
    public static IResult GetAll()
    {
        var access = new TicketAccess();
        var list = access.GetTickets();

        return Results.Ok(list);
    }

    public static IResult GetByTenant(string Tenant)
    {
        var access = new TicketAccess();
        var list = access.GetTicketByTenant(Tenant);

        return Results.Ok(list);
    }

    public static IResult Create(Ticket ticket)
    {
        var access = new TicketAccess();
        access.Create(ticket);

        return Results.Ok();
    }
    public static IResult Update(Ticket ticket)
    {
        var access = new TicketAccess();
        access.Update(ticket);

        return Results.Ok();
    }
}