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
        ExelReader exl = new ExelReader("C:/Users/Alexandre Bodin/Documents/PatchServices-main/CSqlManager/CSqlManager/PATCH_SERVICES_DEVELOPPMENTS.xlsx");
        exl.ExtractExel();
        exl.LinkWithDatabase();
        //var web = WebApplication.Create();
        //BrandEndPoints.MapEndPoints(web);
        //web.Run();
        
    }

    
}

