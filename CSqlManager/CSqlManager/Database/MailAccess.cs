using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
namespace CSqlManager;

public class MailAccess : DbAccess
{
    public MailTemplate? GetMailTemplate()
    {
        MailTemplate? requestResult = null;
        
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT id, mail_acknowledge, mail_completed FROM ps_mail ORDER BY id DESC";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                requestResult = new MailTemplate();
                requestResult.Id = (int)getInt(reader, "id", true)!;
                requestResult.MailAcknowledge = getString(reader, "mail_acknowledge", true)!;
                requestResult.MailCompleted = getString(reader, "mail_completed", true)!;
            }
            Close(Connection);
        }
        
        return requestResult;
    }
    
    public void Create(MailTemplate mail)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);

            command.CommandText = $"INSERT INTO ps_mail (mail_acknowledge, mail_completed) VALUES (@mailAcknowledge, @mailCompleted)";
            command.Parameters.AddWithValue("mailAcknowledge", mail.MailAcknowledge);
            command.Parameters.AddWithValue("mailCompleted", mail.MailCompleted);
            command.ExecuteNonQuery();
            Close(Connection);
        }
    }
    public void Update(MailTemplate mail)
    {
        if (mail == null) {
            return;
        }
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = "UPDATE ps_mail SET mail_acknowledge = @mailAcknowledge, mail_completed = @mailCompleted WHERE id = @id";
            
            command.Parameters.AddWithValue("mailAcknowledge", mail.MailAcknowledge);
            command.Parameters.AddWithValue("mailCompleted", mail.MailCompleted);
            command.Parameters.AddWithValue("id", GetParam(mail.Id)); 
            command.ExecuteNonQuery();
            Close(Connection);
        }
    }
}