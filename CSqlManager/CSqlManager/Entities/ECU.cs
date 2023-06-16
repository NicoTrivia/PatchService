namespace CSqlManager;

public class ECU
{
    public string Brand_code { get; set; }
    public string ECU_code { get; set; }
    
    public string Fuel { get; set; }
    public bool[] toggles { get; set; }

    public ECU() { }

    public ECU(string brandCode, string ecuCode)
    {
        Brand_code = brandCode;
        ECU_code = ecuCode;
        Fuel = "X";
        toggles = new bool[28];
    }
    
    public ECU(string brandCode, string ecuCode, bool[] toToggle)
    {
        Brand_code = brandCode;
        ECU_code = ecuCode;
        Fuel = "X";
        toggles = toToggle;
    }
    
    public ECU(string brandCode, string ecuCode, string fuel, bool[] toToggle)
    {
        Brand_code = brandCode;
        ECU_code = ecuCode;
        Fuel = fuel;
        toggles = toToggle;
    }

    public override string ToString()
    {
        return $"{Brand_code}|{ECU_code}";
    }
}