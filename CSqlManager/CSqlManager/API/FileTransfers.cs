
namespace CSqlManager;

public class FileTransfers
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/files/{fileName}", GetFile);
        app.MapPost("/files/{fileName}", PostFile);
    }
    
    // To change depending on the context
    private static readonly string UploadDirectory = "C:/Users/Alexandre Bodin/Documents/PatchServices-main/CSqlManager/CSqlManager/Files" ;

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

    static Task PostFile(HttpContext context, string fileName)
    {
        var file = context.Request.Form.Files.FirstOrDefault();

        if (file == null || file.Length == 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return Task.CompletedTask;
        }
        
        var filePath = Path.Combine(UploadDirectory, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        context.Response.StatusCode = StatusCodes.Status201Created;
        context.Response.Headers["Location"] = $"/api/files/{fileName}";

        return Task.CompletedTask;
    }
}