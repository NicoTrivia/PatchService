namespace CSqlManager;

public class JwtClaims
{
    public bool Valid { get; set; }
    public string Tenant { get; set; }
    public string User { get; set; }
    public int UserId { get; set; }
    
    public string UserEmail { get; set; }
    
    public string Profile { get; set; }
    public string APP { get; set; }

    public override string ToString()
    {
        return "Tenant: "+Tenant+", User: "+User+", UserId: "+UserId+", UserEmail: "+UserEmail+", Profile: "+Profile+" APP: "+ APP+", Valid: "+Valid;
    }
}