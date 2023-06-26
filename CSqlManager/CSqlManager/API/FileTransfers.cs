
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
        var file = context.Request.Form.Files.FirstOrDefault();

        if (file == null || file.Length == 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return Task.CompletedTask;
        }
        var fileName = "file.bin";
        var tenant = "";
        var ticket_id = "";
        foreach (var formPart in context.Request.Form) {
            if (formPart.Key == "filename") {
               fileName = formPart.Value;
            }
            if (formPart.Key == "ticket_id") {
               ticket_id = formPart.Value;
            }
            if (formPart.Key == "tenant") {
               tenant = formPart.Value;
            }
        }
        Console.WriteLine(fileName+" "+ticket_id+" "+tenant);
        
        var filePath = Path.Combine(UploadDirectory, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        context.Response.StatusCode = StatusCodes.Status201Created;
        context.Response.Headers["Location"] = $"/api/files/{fileName}";

        return Task.CompletedTask;
    }
/*
    public string BuildDirectory( ) {
        <baseDir>/<Tenant>/<annee>/<mois>/<id_ticket>


        "C:\temp\ACME\2023\06\123456.jpg"
    }*/
}