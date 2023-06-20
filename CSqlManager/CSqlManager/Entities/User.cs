namespace CSqlManager;

public class User
{
    public int id { get; set; }
    public string login { get; set; }
    public string lastname { get; set; }
    public string firstname { get; set; }
    public string email { get; set; }
    public string password { get; set; } // encrypted 
    public string tenant  { get; set; }
    public bool active  { get; set; }
    public string? jwt  { get; set; }
}