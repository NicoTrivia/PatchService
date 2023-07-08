namespace CSqlManager;

public class JwtClaims
{
    public bool Valid { get; set; }
    public string Tenant { get; set; }
    public string User { get; set; }
    public string Profile { get; set; }
    public string APP { get; set; }

    public override string ToString()
    {
        return "Tenant: "+Tenant+", User: "+User+", Profile: "+Profile+" APP: "+ APP+", Valid: "+Valid;
    }
}