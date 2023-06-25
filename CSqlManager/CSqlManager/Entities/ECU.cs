namespace CSqlManager;

public class ECU
{
    public string Brand_code { get; set; }
    public string code { get; set; }
    public string Fuel { get; set; }
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

    public ECU(string brandCode, string ecuCode, bool[] toToggle)
    {
        Brand_code = brandCode;
        code = ecuCode;
        Fuel = "X";

        dpf = toToggle[0];
        egr =  toToggle[1];

        lambda = toToggle[2];
        hotstart =  toToggle[3];
        flap =  toToggle[4];
        dtc =  toToggle[5];
        adblue =  toToggle[6];
        torqmonitor = toToggle[7];

        speedlimit =  toToggle[8];
        startstop =  toToggle[9];
        nox =  toToggle[10];
        tva =  toToggle[11];
        readiness =  toToggle[12];
        immo = toToggle[13];

        maf = toToggle[14];
        hardcut = toToggle[15];
        displaycalibration = toToggle[16];
        waterpump = toToggle[17];
        tprot =  toToggle[18];

        o2 = toToggle[19];
        glowplugs = toToggle[20];
        y75 = toToggle[21];
        special =  toToggle[22];
        decata =  toToggle[23];
        vmax = toToggle[24];
        stage1 = toToggle[25];

        stage2 =  toToggle[26];
        flexfuel = toToggle[27];
        
    }
    
    public ECU(string brandCode, string ecuCode, string fuel, bool[] toToggle)
    {
        Brand_code = brandCode;
        code = ecuCode;
        Fuel = fuel;
        
        dpf = toToggle[0];
        egr =  toToggle[1];

        lambda = toToggle[2];
        hotstart =  toToggle[3];
        flap =  toToggle[4];
        dtc =  toToggle[5];
        adblue =  toToggle[6];
        torqmonitor = toToggle[7];

        speedlimit =  toToggle[8];
        startstop =  toToggle[9];
        nox =  toToggle[10];
        tva =  toToggle[11];
        readiness =  toToggle[12];
        immo = toToggle[13];

        maf = toToggle[14];
        hardcut = toToggle[15];
        displaycalibration = toToggle[16];
        waterpump = toToggle[17];
        tprot =  toToggle[18];

        o2 = toToggle[19];
        glowplugs = toToggle[20];
        y75 = toToggle[21];
        special =  toToggle[22];
        decata =  toToggle[23];
        vmax = toToggle[24];
        stage1 = toToggle[25];

        stage2 =  toToggle[26];
        flexfuel = toToggle[27];
       
    }

    public override string ToString()
    {
        return $"{Brand_code}|{code}";
    }
}