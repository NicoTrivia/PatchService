
namespace CSqlManager;

public class FileTransfers
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/files/{fileName}", GetFile);
        app.MapPost("/files", PostFile);
    }
    
    // To change depending on the context
    private static readonly string UploadDirectory = "C:/temp" ;

    // Needs an update 
    static Task GetFile(HttpContext context, string fileName)
    {
        string? name = context.Request.RouteValues[fileName] as string;
        var filePath = Path.Combine(UploadDirectory, name);
        
        if (!File.Exists(filePath))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return Task.CompletedTask;
        }
        
        context.Response.ContentType = "application/octet-stream";
        context.Response.Headers["Content-Disposition"] = $"attachment; filename=\"{name}\"";

        using (var fileStream = File.OpenRead(filePath))
        {
            fileStream.CopyTo(context.Response.Body);
        }
        
        return Task.CompletedTask;
    }

    static Task PostFile(HttpContext context)
    {
        Console.WriteLine("Post treatment of a file :");
        var file = context.Request.Form.Files.FirstOrDefault();

        if (file == null || file.Length == 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine("ERROR 400 : No file was found");
            return Task.CompletedTask;
        }
        var fileName = "file.bin";
        var tenant = "";
        var ticket_id = "";
        foreach (var formPart in context.Request.Form) {
            if (formPart.Key == "filename") {
               fileName = formPart.Value;
            }
            else if (formPart.Key == "ticket_id") {
               ticket_id = formPart.Value;
            }
            else if (formPart.Key == "tenant") {
               tenant = formPart.Value;
            }
            else
            {
                Console.WriteLine("WARNING UNKNOWN PARAMETER :" + formPart.Key + " with value :" + formPart.Value);
            }
        }
        Console.WriteLine("file informations : " + fileName+" "+ticket_id+" "+tenant);
        
        string fileLocation = BuildDirectory(tenant, ticket_id, fileName);
        var filePath = Path.Combine(fileLocation, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        context.Response.StatusCode = StatusCodes.Status201Created;
        context.Response.Headers["Location"] = fileLocation;
        Console.WriteLine("File Saved in : " + fileLocation);
        Console.WriteLine();
        return Task.CompletedTask;
    }

    public static string BuildDirectory(string tenant, string ticketID, string filename)
    {
        string Dlocation = $"{UploadDirectory}/{tenant}/{ticketID}";
        if (!Path.Exists(Dlocation))
        {
            Directory.CreateDirectory(Dlocation);
        }

        return Dlocation + $"/{filename}";
    }
}