// See https://aka.ms/new-console-template for more information


// http://localhost:8080

//APISender.SendOn8080("code", exl);
//exl.LinkWithDatabase();

using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

using CSqlManager;
using Microsoft.AspNetCore.Mvc.Routing;

class Program
{
    static void Main()
    {
        ExelReader exl = new ExelReader("C:/Users/Alexandre/Documents/CSqlManager/CSqlManager/PATCH_SERVICES_DEVELOPPMENTS.xlsx");
        exl.ExtractExel();
        
        APISender myController = new APISender();
        myController.exelReader = exl;
        
        
        IActionResult result = myController.GetInBrand("code");
        Console.WriteLine(myController);
        if (result is OkObjectResult okObjectResult)
        {
            var responseContent = okObjectResult.Value;
           var web = WebApplication.Create();
            web.MapGet("brand", () => responseContent);
            web.Run();
            
        }
        else
        {
            Console.WriteLine("Request failed.");
        }
        
    }

    
}

