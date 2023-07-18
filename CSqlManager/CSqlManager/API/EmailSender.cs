using System;
using System.Net;
using System.Net.Mail;
using System.Globalization;
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

    private static string ReplaceAll(string body, Ticket ticket)
    {
        string[] name = ticket.user_name!.Split(" ");
        CultureInfo culture = new CultureInfo("fr-FR");
        DateTime dateTicket = (ticket.processed_file_name == null) ? TimeZoneInfo.ConvertTimeFromUtc(ticket.date.Value, TimeZoneInfo.Local) : ticket.date.Value;
        
        body = body.Replace("${ticket_creation_date}", dateTicket.ToString("F", culture));
        body = body.Replace("${ticket_id}", ticket.id.ToString());
        body = body.Replace("${customer_name}", ticket.tenant);
        body = body.Replace("${immatriculation}", ticket.immatriculation == null ? "" : ticket.immatriculation);
        body = body.Replace("${user_first_name}", name[0]);
        body = body.Replace("${user_last_name}", name[1]);
        body = body.Replace("${comment}", ticket.comment == null ? "" : ticket.comment);
        body = body.Replace("${brand_code}", ticket.brand_code);
        body = body.Replace("${brand_name}", ticket.brand_name);
        body = body.Replace("${ecu_code}", ticket.ecu_code);
        body = body.Replace("${engine}", ticket.fuel == null ? "" : ticket.fuel );
        body = body.Replace("${processed_user_name}", ticket.processed_user_name == null ? "" :  ticket.processed_user_name );
        body = body.Replace("<p>", "");
        body = body.Replace("</p>", "<br/>");
        
        return body;
    }

    public static void Send(bool acknowledge, string recipientEmail, Ticket ticket)
    {
        MailAccess access = new MailAccess();
        MailTemplate? mailTemplate = access.GetMailTemplate();
        string subject = acknowledge ? Variables.RetrieveVariable("mail_subject_acknowledge") : Variables.RetrieveVariable("mail_subject_completed");
        if (mailTemplate == null) {
            MyLogManager.Error("Mail template is not set.");
            return;
        }
        string body = acknowledge ? mailTemplate.MailAcknowledge : mailTemplate.MailCompleted;
        body = ReplaceAll(body, ticket);
        
        MailMessage message = new MailMessage(senderEmail, recipientEmail, subject, body);
        message.IsBodyHtml = true; 

        MyLogManager.Debug($"Email sent to {recipientEmail} with server : {smtpServer}/ port : {smtpPort}/sender : {senderEmail}");
        SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

        try
        {
            smtpClient.Send(message);
            MyLogManager.Debug("Email sent successfully.");
        }
        catch (Exception ex)
        {
            MyLogManager.Error("An error occurred while sending the email: " + ex.Message);
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
        MyLogManager.Debug($"MAIL POST {mail}");
       
        var access = new MailAccess();
        access.Create(mail);
        MyLogManager.Debug($"Mail created : {mail.Id} by {claims.User} / {claims.Tenant}");
        return Results.Ok(mail);
    }
    public static IResult UpdateMailTemplate(HttpContext context, MailTemplate mail)
    {
        JwtClaims claims = getJwtClaims(context);
        if (!claims.Valid || (claims.Profile != "ADMIN")) {
            MyLogManager.Error("ERROR 401 : Invalid JWT/PROFILE : "+ claims);
            return Results.Unauthorized();
        }
        MyLogManager.Debug($"MAIL PUT {mail}");
        
        var access = new MailAccess();
        access.Update(mail);
        MyLogManager.Debug($"Mail updated : {mail.Id} by {claims.User} / {claims.Tenant}");
        return Results.Ok(mail);
    }
}