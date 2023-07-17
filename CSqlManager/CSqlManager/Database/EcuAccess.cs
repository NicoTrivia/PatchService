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

            MyLogManager.Log("BrandCode : "+BrandCode);
            MyLogManager.Log("Fuel : "+Fuel);

            MyLogManager.Log(command.CommandText);
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
    
    public void Create(ECU ecu)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"INSERT INTO ps_ecu (brand_code, code, carburant,dpf, egr, " +
                                              $"lambda, hotstart, flap, adblue, dtc, torqmonitor, " +
                                              $"speedlimit, startstop, nox, tva, readiness, immo, " +
                                              $"maf, hardcut, displaycalibration, waterpump, tprot, " +
                                              $"o2, glowplugs, y75, special, decata, vmax, stage1, " +
                                              $"stage2, flexfuel) " +
                                              $"VALUES (@brandCode, @code, @fuel,@dpf, @egr, @lambda, @hotstart, @flap, @adblue, " +
                                              $"@dtc, @torqmonitor, @speedlimit, @startstop, @nox, @tva, @readiness, @immo, " +
                                              $"@maf, @hardcut, @displaycalibration, @waterpump, " +
                                              $"@tprot, @o2, @glowplugs, @y75, @special, @decata, @vmax, @stage1, @stage2, @flexfuel)";

            command.Parameters.AddWithValue("brandCode", ecu.Brand_code);
            command.Parameters.AddWithValue("code", ecu.code);
            command.Parameters.AddWithValue("fuel", ecu.Fuel);

            command.Parameters.AddWithValue("dpf", ecu.dpf);
            command.Parameters.AddWithValue("egr", ecu.egr);
            command.Parameters.AddWithValue("lambda", ecu.lambda);
            command.Parameters.AddWithValue("hotstart", ecu.hotstart);
            command.Parameters.AddWithValue("flap", ecu.flap);
            command.Parameters.AddWithValue("adblue", ecu.adblue);

            command.Parameters.AddWithValue("dtc", ecu.dtc);
            command.Parameters.AddWithValue("torqmonitor", ecu.torqmonitor);
            command.Parameters.AddWithValue("speedlimit", ecu.speedlimit);
            command.Parameters.AddWithValue("startstop", ecu.startstop);
            command.Parameters.AddWithValue("nox", ecu.nox);
            command.Parameters.AddWithValue("tva", ecu.tva);


            command.Parameters.AddWithValue("readiness", ecu.readiness);
            command.Parameters.AddWithValue("immo", ecu.immo);
            command.Parameters.AddWithValue("maf", ecu.maf);
            command.Parameters.AddWithValue("hardcut", ecu.hardcut);
            command.Parameters.AddWithValue("displaycalibration", ecu.displaycalibration);
            command.Parameters.AddWithValue("waterpump", ecu.waterpump);
            command.Parameters.AddWithValue("tprot", ecu.tprot);

            command.Parameters.AddWithValue("o2", ecu.o2);
            command.Parameters.AddWithValue("glowplugs", ecu.glowplugs);
            command.Parameters.AddWithValue("y75", ecu.y75);
            command.Parameters.AddWithValue("special", ecu.special);
            command.Parameters.AddWithValue("decata", ecu.decata);
            command.Parameters.AddWithValue("vmax", ecu.vmax);
            command.Parameters.AddWithValue("stage1", ecu.stage1);
            command.Parameters.AddWithValue("stage2", ecu.stage2);
            command.Parameters.AddWithValue("flexfuel", ecu.flexfuel);
            command.ExecuteNonQuery();
        }
    }
    
    public void Update(ECU ecu)
    {
        if (ecu == null) {
            return;
        }
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = "UPDATE ps_ecu SET carburant= @fuel, dpf=@dpf, egr=@egr," +
                                              $" lambda=@lambda, hotstart=@hotstart, flap=@flap, adblue=@adblue, dtc=@dtc, torqmonitor=@torqmonitor," +
                                              $" speedlimit=@speedlimit, startstop=@startstop, nox=@nox, tva=@tva, readiness=@readiness, immo=@immo," +
                                              $" maf=@maf, hardcut=@hardcut, displaycalibration=@displaycalibration, waterpump=@waterpump, tprot=@tprot," +
                                              $" o2=@o2, glowplugs=@glowplugs, y75=@y75, special=@special, decata=@decata, vmax=@vmax, stage1=@stage1," +
                                              $" stage2=@stage2, flexfuel=@flexfuel WHERE brand_code=@brandCode and code=@code";


            command.Parameters.AddWithValue("brandCode", ecu.Brand_code);
            command.Parameters.AddWithValue("code", ecu.code);
            command.Parameters.AddWithValue("fuel", ecu.Fuel);

            command.Parameters.AddWithValue("dpf", ecu.dpf);
            command.Parameters.AddWithValue("egr", ecu.egr);

            command.Parameters.AddWithValue("lambda", ecu.lambda);
            command.Parameters.AddWithValue("hotstart", ecu.hotstart);
            command.Parameters.AddWithValue("flap", ecu.flap);
            command.Parameters.AddWithValue("adblue", ecu.adblue);
            command.Parameters.AddWithValue("dtc", ecu.dtc);
            command.Parameters.AddWithValue("torqmonitor", ecu.torqmonitor);


            command.Parameters.AddWithValue("speedlimit", ecu.speedlimit);
            command.Parameters.AddWithValue("startstop", ecu.startstop);
            command.Parameters.AddWithValue("nox", ecu.nox);
            command.Parameters.AddWithValue("tva", ecu.tva);
            command.Parameters.AddWithValue("readiness", ecu.readiness);
            command.Parameters.AddWithValue("immo", ecu.immo);

            command.Parameters.AddWithValue("maf", ecu.maf);
            command.Parameters.AddWithValue("hardcut", ecu.hardcut);
            command.Parameters.AddWithValue("displaycalibration", ecu.displaycalibration);
            command.Parameters.AddWithValue("waterpump", ecu.waterpump);
            command.Parameters.AddWithValue("tprot", ecu.tprot);

            command.Parameters.AddWithValue("o2", ecu.o2);
            command.Parameters.AddWithValue("glowplugs", ecu.glowplugs);
            command.Parameters.AddWithValue("y75", ecu.y75);
            command.Parameters.AddWithValue("special", ecu.special);
            command.Parameters.AddWithValue("decata", ecu.decata);
            command.Parameters.AddWithValue("vmax", ecu.vmax);
            command.Parameters.AddWithValue("stage1", ecu.stage1);
            command.Parameters.AddWithValue("stage2", ecu.stage2);
            command.Parameters.AddWithValue("flexfuel", ecu.flexfuel);

            /*
            command.CommandText = "UPDATE ps_ecu SET carburant= @fuel, dpf=@dpf, egr=@egr," +
                                              $" lambda=@lambda, hotstart=@hotstart, flap=@flap, adblue=@adblue, dtc=@dtc, torqmonitor=@torqmonitor," +
                                              $" speedlimit=@speedlimit, startstop=@startstop, nox=@nox, tva=@tva, readines=@readinesss, immo=@immo," +
                                              $" maf=@maf, hardcut=@hardcut, displaycalibration=@displaycalibration, waterpump=@waterpump, tprot=@tprot," +
                                              $" o2=@o2, glowplugs=@glowplugs, y75=@y75, special=@special, decata=@decata, vmax=@vmax, stage1=@stage1," +
                                              $" stage2=@stage2, flexfuel=@flexfuel WHERE brand_code=@brandCode and code=@code";

            
            */

            command.ExecuteNonQuery();
        }
    }

    public Boolean DeleteBrandByCode(string brand_code, string code) {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"DELETE FROM ps_ecu WHERE brand_code = @brandCode and code = @code";
            
            command.Parameters.AddWithValue("brandCode", brand_code);
            command.Parameters.AddWithValue("code", code);
            int count = command.ExecuteNonQuery();
            
            return count > 0;
        }
    }

    public Boolean DeleteByBrandCode(string brand_code) {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"DELETE FROM ps_ecu WHERE brand_code = @brandCode";
            
            command.Parameters.AddWithValue("brandCode", brand_code);
            int count = command.ExecuteNonQuery();
            
            return count > 0;
        }
    }
}