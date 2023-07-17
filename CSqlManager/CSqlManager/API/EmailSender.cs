using System;
using System.Net;
using System.Net.Mail;

namespace CSqlManager;

public class EmailSender
{
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
}