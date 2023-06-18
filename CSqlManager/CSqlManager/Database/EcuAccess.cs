using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
namespace CSqlManager;


public class EcuAccess : DbAccess
{
    public List<ECU> GetEcuByBrandCode(string BrandCode, string? Fuel)
    {
        List<ECU> requestResult = new List<ECU>();
        
        using (NpgsqlCommand command = CreateCommand())
        {
            if (command.Parameters.Contains("BrandCode"))
                command.Parameters.Remove("BrandCode");
            if (command.Parameters.Contains("Fuel"))
                command.Parameters.Remove("Fuel");

            command.CommandText = $"SELECT code, carburant, dpf, egr, " +
                                  $" lambda, hotstart, flap, adblue, dtc, torqmonitor," +
                                  $" speedlimit, startstop, nox, tva, readiness, immo," +
                                  $" maf, hardcut, displaycalibration, waterpump, tprot," +
                                  $" o2, glowplugs, y75, special, decata, vmax, stage1," +
                                  $" stage2, flexfuel FROM ps_ecu WHERE brand_code = @BrandCode";

            command.Parameters.AddWithValue("BrandCode", BrandCode);

            if (Fuel != null)
            {
                command.CommandText += $" AND carburant = @Fuel";
                command.Parameters.AddWithValue("Fuel", Fuel);

            }
            command.CommandText += $" ORDER BY code";

            Console.WriteLine("BrandCode : "+BrandCode);
            Console.WriteLine("Fuel : "+Fuel);

            Console.WriteLine(command.CommandText);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ECU ecu = new ECU();
                ecu.Brand_code =  BrandCode;
                ecu.code =  reader.GetString(reader.GetOrdinal("code"));
                ecu.Fuel =  reader.GetString(reader.GetOrdinal("carburant"));

                ecu.dpf =  reader.GetBoolean(reader.GetOrdinal("dpf"));
                ecu.egr =  reader.GetBoolean(reader.GetOrdinal("egr"));

                ecu.lambda =  reader.GetBoolean(reader.GetOrdinal("lambda"));
                ecu.hotstart =  reader.GetBoolean(reader.GetOrdinal("hotstart"));
                ecu.flap =  reader.GetBoolean(reader.GetOrdinal("flap"));
                ecu.dtc =  reader.GetBoolean(reader.GetOrdinal("dtc"));
                ecu.adblue =  reader.GetBoolean(reader.GetOrdinal("adblue"));
                ecu.torqmonitor =  reader.GetBoolean(reader.GetOrdinal("torqmonitor"));

                ecu.speedlimit =  reader.GetBoolean(reader.GetOrdinal("speedlimit"));
                ecu.startstop =  reader.GetBoolean(reader.GetOrdinal("startstop"));
                ecu.nox =  reader.GetBoolean(reader.GetOrdinal("nox"));
                ecu.tva =  reader.GetBoolean(reader.GetOrdinal("tva"));
                ecu.readiness =  reader.GetBoolean(reader.GetOrdinal("readiness"));
                ecu.immo =  reader.GetBoolean(reader.GetOrdinal("immo"));

                ecu.maf =  reader.GetBoolean(reader.GetOrdinal("maf"));
                ecu.hardcut =  reader.GetBoolean(reader.GetOrdinal("hardcut"));
                ecu.displaycalibration =  reader.GetBoolean(reader.GetOrdinal("displaycalibration"));
                ecu.waterpump =  reader.GetBoolean(reader.GetOrdinal("waterpump"));
                ecu.tprot =  reader.GetBoolean(reader.GetOrdinal("tprot"));

                ecu.o2 =  reader.GetBoolean(reader.GetOrdinal("o2"));
                ecu.glowplugs =  reader.GetBoolean(reader.GetOrdinal("glowplugs"));
                ecu.y75 =  reader.GetBoolean(reader.GetOrdinal("y75"));
                ecu.special =  reader.GetBoolean(reader.GetOrdinal("special"));
                ecu.decata =  reader.GetBoolean(reader.GetOrdinal("decata"));
                ecu.vmax =  reader.GetBoolean(reader.GetOrdinal("vmax"));
                ecu.stage1 =  reader.GetBoolean(reader.GetOrdinal("stage1"));

                ecu.stage2 =  reader.GetBoolean(reader.GetOrdinal("stage2"));
                ecu.flexfuel =  reader.GetBoolean(reader.GetOrdinal("flexfuel"));

                 requestResult.Add(ecu);
            }

        }
        
        return requestResult;
    }
    
}