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
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        TicketAccess access = new TicketAccess();
        var list = access.GetTickets();

        return Results.Ok(list);
    }

    public static IResult GetInProgress(HttpContext context)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        TicketAccess access = new TicketAccess();
        var list = access.GetInProgress(claims.UserId);

        return Results.Ok(list);
    }
    public static IResult GetByTenant(HttpContext context, string Tenant)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || claims.Tenant != Tenant) {
            MyLogManager.Error("ERROR 401 : Invalid JWT : "+ claims);
            return Results.Unauthorized();
        }
        TicketAccess access = new TicketAccess();
        var list = access.GetByTenant(Tenant);

        return Results.Ok(list);
    }

    public static IResult GetById(HttpContext context, int id)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid) {
            MyLogManager.Error("ERROR 401 : Invalid JWT : "+ claims);
            return Results.Unauthorized();
        }
        TicketAccess access = new TicketAccess();
        var ticket = access.GetById(id);

        if (ticket != null && (claims.Tenant != ticket.tenant) && (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            MyLogManager.Error("ERROR 401 : Invalid tenant : "+ claims);
            return Results.Unauthorized();
        }
        return Results.Ok(ticket);
    }

    public static IResult Create(HttpContext context, Ticket ticket)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || claims.Tenant != ticket.tenant) {
            MyLogManager.Error("ERROR 401 : Invalid JWT : "+ claims);
            return Results.Unauthorized();
        }

        if (!checkFileId(ticket)) {
            return Results.BadRequest("Invalid file id: "+ticket.file_id+" for tenant: "+claims.Tenant );
        }

        TicketAccess access = new TicketAccess();
        access.Create(ticket);        
        MyLogManager.Debug($"Ticket Created : {ticket.id} by {claims.User} / {claims.Tenant} / {claims.UserEmail}");
        if (claims.UserEmail != null && !claims.UserEmail.StartsWith("#")) {
            EmailSender.Send(true, claims.UserEmail, ticket);
        }
        return Results.Ok(ticket);
    }
    public static IResult Update(HttpContext context,Ticket ticket)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN" && claims.Profile != "OPERATOR")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        TicketAccess access = new TicketAccess();
        access.Update(ticket);
        MyLogManager.Debug($"Ticket Updated : {ticket.id} by {claims.User} / {claims.Tenant}");
        UserAccess userAccess = new UserAccess();
        User user = userAccess.GetUserById((int)ticket.user_id!);
        if (user != null && user.email != null && !user.email.StartsWith("#") && ticket.processed_user_name != null 
            && ticket.processed_file_name != null) {

            EmailSender.Send(false, user.email, ticket);
        }
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
                    MyLogManager.Warn("MISSING Read folder is : "+directory);
                }
            }
            return true;
        }
        return false;
    }
}