// Exel code if needed :
/* ExelReader exl = new ExelReader("C:/Users/Alexandre Bodin/Documents/PatchServices-main/CSqlManager/CSqlManager/PATCH_SERVICES_DEVELOPPMENTS.xlsx");
exl.ExtractExel();
exl.LinkWithDatabase();*/

using CSqlManager;

class Program
{
    static void Main()
    {
       // IMPORTANT : Make sure to put the .properties ans the log4net.config files with the final executable 

       // Web management 
       string[] args = null;
       var builder = WebApplication.CreateBuilder(args);
       // WARNING : Rememeber to change the file upload directory depending on the context
       /*
       MyLogManager.Log("Lancement de PatchServices Bakend");
       EmailSender.Send("alexandre.bodin78@free.fr", "Test1");
       */
       builder.Services.AddCors(options =>
       {
           options.AddDefaultPolicy(builder =>
           {
               builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
           });
       });
      
       var web = builder.Build();
       
       // Register CORS services
       web.UseCors();
       mapEndPoints(web);
       web.Run();
    }

    static void mapEndPoints(WebApplication web) {
        BrandEndPoints.MapEndPoints(web);
        EcuEndPoints.MapEndPoints(web);
        TenantEndPoints.MapEndPoints(web);
        UserEndPoints.MapEndPoints(web);
        FileTransfers.MapEndPoints(web);
        TicketEndPoints.MapEndPoints(web);
        EmailSender.MapEndPoints(web);
    }
}

