namespace CSqlManager;
using System.Net;
public class TicketEndPoints: SecureEnpoint
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/ticket", GetAll);
        app.MapGet("/ticket/{id}", GetById);
        app.MapGet("/ticket_in_progress", GetInProgress);
        app.MapGet("/ticket_list/{Tenant}", GetByTenant);
        app.MapPost("/ticket", Create);
        app.MapPut("/ticket", Update);
    }
    
    public static IResult GetAll(HttpContext context)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            Console.WriteLine("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TicketAccess();
        var list = access.GetTickets();

        return Results.Ok(list);
    }

    public static IResult GetInProgress(HttpContext context)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            Console.WriteLine("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TicketAccess();
        var list = access.GetInProgress(claims.UserId);

        return Results.Ok(list);
    }
    public static IResult GetByTenant(HttpContext context, string Tenant)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || claims.Tenant != Tenant) {
            Console.WriteLine("ERROR 401 : Invalid JWT : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TicketAccess();
        var list = access.GetByTenant(Tenant);

        return Results.Ok(list);
    }

    public static IResult GetById(HttpContext context, int id)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid) {
            Console.WriteLine("ERROR 401 : Invalid JWT : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TicketAccess();
        var ticket = access.GetById(id);

        if (ticket != null && (claims.Tenant != ticket.tenant) && (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            Console.WriteLine("ERROR 401 : Invalid tenant : "+ claims);
            return Results.Unauthorized();
        }
        return Results.Ok(ticket);
    }

    public static IResult Create(HttpContext context, Ticket ticket)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || claims.Tenant != ticket.tenant) {
            Console.WriteLine("ERROR 401 : Invalid JWT : "+ claims);
            return Results.Unauthorized();
        }

        if (!checkFileId(ticket)) {
            return Results.BadRequest("Invalid file id: "+ticket.file_id+" for tenant: "+claims.Tenant );
        }

        var access = new TicketAccess();
        access.Create(ticket);

        return Results.Ok(ticket);
    }
    public static IResult Update(HttpContext context,Ticket ticket)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            Console.WriteLine("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access = new TicketAccess();
        access.Update(ticket);

        return Results.Ok(ticket);
    }

    protected static bool checkFileId( Ticket ticket) {
        if (ticket != null && ticket.tenant != null && ticket.file_id != null) {
            int fileId = -1;
            if (int.TryParse(ticket.file_id, out fileId)) {
                String directory = FileTransfers.BuildDirectory(ticket.tenant, fileId);
                if (Directory.Exists(directory))
                {
                    ticket.file_id = FileTransfers.BuildFileId(ticket.tenant, fileId);
                } else {
                    Console.WriteLine("MISSING Read folder is : "+directory);
                }
            }
            return true;
        }
        return false;
    }
}