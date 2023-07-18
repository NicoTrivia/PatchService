using System;
using System.Net;
using System.Net.Mail;

namespace CSqlManager;

public class EmailSender: SecureEnpoint
{
    public static void MapEndPoints(WebApplication app)
    {
        app.MapGet("/mail_template", GetMailTemplate);
        app.MapPost("/mail_template", CreateMailTemplate);
        app.MapPut("/mail_template", UpdateMailTemplate);
    }
    
    static string smtpServer = (string)Variables.RetrieveVariable("stmpServer");
    static int smtpPort = Convert.ToInt32(Variables.RetrieveVariable("stmpPort"));
    static string senderEmail = (string)Variables.RetrieveVariable("senderEmail");
    static string senderPassword = (string)Variables.RetrieveVariable("senderPassword");

    private static string body = File.ReadAllText("template_mail_qualify.html");

    public static void Send(string recipientEmail, string subject)
    {
        MailMessage message = new MailMessage(senderEmail, recipientEmail, subject, body);
        message.IsBodyHtml = true; 

        MyLogManager.Log($"Email sent to {recipientEmail} with server : {smtpServer}/ port : {smtpPort}/sender : {senderEmail}");
        SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

        try
        {
            smtpClient.Send(message);
            MyLogManager.Log("Email sent successfully.");
        }
        catch (Exception ex)
        {
            MyLogManager.Log("An error occurred while sending the email: " + ex.Message);
        }
    }

    public static IResult GetMailTemplate(HttpContext context)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        var access = new MailAccess();
        var mailTemplate = access.GetMailTemplate();

        return Results.Ok(mailTemplate);
    }
    
    public static IResult CreateMailTemplate(HttpContext context, MailTemplate mail)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Log($"MAIL POST {mail}");
       
        var access = new MailAccess();
        access.Create(mail);
        MyLogManager.Log($"Mail created : {mail.Id} by {claims.User} / {claims.Tenant}");
        return Results.Ok(mail);
    }
    public static IResult UpdateMailTemplate(HttpContext context, MailTemplate mail)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Log($"MAIL PUT {mail}");
        
        var access = new MailAccess();
        access.Update(mail);
        MyLogManager.Log($"Mail updated : {mail.Id} by {claims.User} / {claims.Tenant}");
        return Results.Ok(mail);
    }
}