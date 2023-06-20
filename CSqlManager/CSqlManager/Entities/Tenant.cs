namespace CSqlManager;

public class Tenant
{
    public enum Levels
    {
        Silver,Gold,Platine // and so on ..
    }
    
    public string code { get; set; }
    public string name { get; set; }
    
    public string email { get; set; }

    public string Level { get; set; }
    
    public string creation_date { get; set; }
    public string expiration_date { get; set; }
    
    public bool active  { get; set; }
}