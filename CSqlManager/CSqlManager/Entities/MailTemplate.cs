namespace CSqlManager;

public class MailTemplate
{
    public int Id { get; set; }
    
    public string MailAcknowledge { get; set; }
    public string MailCompleted { get; set; }

    public MailTemplate() {}
    
    public override string ToString()
    {
        return Id.ToString();
    }
}