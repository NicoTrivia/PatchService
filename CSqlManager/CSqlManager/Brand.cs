namespace CSqlManager;

public class Brand
{
    public string Code { get; set; }
    public string Name { get; set; }

    public Brand(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}