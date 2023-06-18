namespace CSqlManager;

public class ECU
{
    public string Brand_code { get; set; }
    public string code { get; set; }
    
    public string Fuel { get; set; }
    public bool[] toggles { get; set; }

    public bool dpf { get; set; }
    public bool egr { get; set; }
    public bool lambda { get; set; }
    public bool hotstart { get; set; }
    public bool flap { get; set; }
    public bool adblue { get; set; }
    public bool dtc { get; set; }
    public bool torqmonitor { get; set; }
    public bool speedlimit { get; set; }
    public bool startstop { get; set; }
    public bool nox { get; set; }
    public bool tva { get; set; }
    public bool readiness { get; set; }
    public bool immo { get; set; }
    public bool maf { get; set; }
    public bool hardcut { get; set; }
    public bool displaycalibration { get; set; }
    public bool waterpump { get; set; }
    public bool tprot { get; set; }
    public bool o2 { get; set; }
    public bool glowplugs { get; set; }
    public bool y75 { get; set; }
    public bool special { get; set; }
    public bool decata { get; set; }
    public bool vmax { get; set; }
    public bool stage1 { get; set; }
    public bool stage2 { get; set; }
    public bool flexfuel { get; set; }


    public ECU() { }

    public ECU(string brandCode, string ecuCode)
    {
        Brand_code = brandCode;
        code = ecuCode;
        Fuel = "X";
        toggles = new bool[28];
    }
    
    public ECU(string brandCode, string ecuCode, bool[] toToggle)
    {
        Brand_code = brandCode;
        code = ecuCode;
        Fuel = "X";
        toggles = toToggle;
    }
    
    public ECU(string brandCode, string ecuCode, string fuel, bool[] toToggle)
    {
        Brand_code = brandCode;
        code = ecuCode;
        Fuel = fuel;
        toggles = toToggle;
    }

    public override string ToString()
    {
        return $"{Brand_code}|{code}";
    }
}