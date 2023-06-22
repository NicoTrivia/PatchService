using Npgsql;
namespace CSqlManager;

public class TicketAccess : DbAccess
{
    private void AddFromReader(NpgsqlDataReader reader, Ticket ticket)
    {
        ticket.id =  reader.GetInt32(reader.GetOrdinal("id"));
        ticket.tenant = reader.GetString(reader.GetOrdinal("tenant"));
        ticket.user_level = reader.GetString(reader.GetOrdinal("level"));
        ticket.user_id = reader.GetInt32(reader.GetOrdinal("user_id"));
        ticket.user_name = reader.GetString(reader.GetOrdinal("user_name"));
        ticket.date = reader.GetDateTime(reader.GetOrdinal("date"));
        ticket.file_name = reader.GetString(reader.GetOrdinal("file_name"));
        ticket.file_size = reader.GetInt32(reader.GetOrdinal("file_size"));
        ticket.immatriculation = reader.GetString(reader.GetOrdinal("immatriculation"));
        ticket.fuel = reader.GetString(reader.GetOrdinal("fuel"));
        
        ticket.processed_file_name = reader.GetString(reader.GetOrdinal("processed_file_name"));
        ticket.processed_file_size = reader.GetInt32(reader.GetOrdinal("processed_file_size"));
        ticket.processed_user_name = reader.GetString(reader.GetOrdinal("rocessed_user_name"));
        ticket.processed_user_id = reader.GetInt32(reader.GetOrdinal("processed_user_id"));
        ticket.processed_date = reader.GetDateTime(reader.GetOrdinal("processed_date"));
        
        ticket.brand_code = reader.GetString(reader.GetOrdinal("brand_code"));
        ticket.ecu_code = reader.GetString(reader.GetOrdinal("ecu_code"));
        ticket.brand_name = reader.GetString(reader.GetOrdinal("brand_name"));
    
        ticket.dpf =  reader.GetBoolean(reader.GetOrdinal("dpf"));
        ticket.egr =  reader.GetBoolean(reader.GetOrdinal("egr"));
        ticket.lambda =  reader.GetBoolean(reader.GetOrdinal("lambda"));
        ticket.hotstart =  reader.GetBoolean(reader.GetOrdinal("hotstart"));
        ticket.flap =  reader.GetBoolean(reader.GetOrdinal("flap"));
        ticket.dtc =  reader.GetBoolean(reader.GetOrdinal("dtc"));
        ticket.adblue =  reader.GetBoolean(reader.GetOrdinal("adblue"));
        ticket.torqmonitor =  reader.GetBoolean(reader.GetOrdinal("torqmonitor"));
        ticket.speedlimit =  reader.GetBoolean(reader.GetOrdinal("speedlimit"));
        ticket.startstop =  reader.GetBoolean(reader.GetOrdinal("startstop"));
        ticket.nox =  reader.GetBoolean(reader.GetOrdinal("nox"));
        ticket.tva =  reader.GetBoolean(reader.GetOrdinal("tva"));
        ticket.readiness =  reader.GetBoolean(reader.GetOrdinal("readiness"));
        ticket.immo =  reader.GetBoolean(reader.GetOrdinal("immo"));
        ticket.maf =  reader.GetBoolean(reader.GetOrdinal("maf"));
        ticket.hardcut =  reader.GetBoolean(reader.GetOrdinal("hardcut"));
        ticket.displaycalibration =  reader.GetBoolean(reader.GetOrdinal("displaycalibration"));
        ticket.waterpump =  reader.GetBoolean(reader.GetOrdinal("waterpump"));
        ticket.tprot =  reader.GetBoolean(reader.GetOrdinal("tprot"));
        ticket.o2 =  reader.GetBoolean(reader.GetOrdinal("o2"));
        ticket.glowplugs =  reader.GetBoolean(reader.GetOrdinal("glowplugs"));
        ticket.y75 =  reader.GetBoolean(reader.GetOrdinal("y75"));
        ticket.special =  reader.GetBoolean(reader.GetOrdinal("special"));
        ticket.decata =  reader.GetBoolean(reader.GetOrdinal("decata"));
        ticket.vmax =  reader.GetBoolean(reader.GetOrdinal("vmax"));
        ticket.stage1 =  reader.GetBoolean(reader.GetOrdinal("stage1"));
        ticket.stage2 =  reader.GetBoolean(reader.GetOrdinal("stage2"));
        ticket.flexfuel =  reader.GetBoolean(reader.GetOrdinal("flexfuel"));
        
    }

    private void AddInCommand(NpgsqlCommand command, Ticket ticket)
    {
        command.Parameters.AddWithValue("id", GetParam(ticket.id));
        command.Parameters.AddWithValue("tenant", GetParam(ticket.id));
        command.Parameters.AddWithValue("level", GetParam(ticket.id));
        command.Parameters.AddWithValue("user_id", GetParam(ticket.id));
        command.Parameters.AddWithValue("user_name", GetParam(ticket.id));
        command.Parameters.AddWithValue("date", GetParam(ticket.id));
        command.Parameters.AddWithValue("file_name", GetParam(ticket.id));
        command.Parameters.AddWithValue("file_size", GetParam(ticket.id));
        command.Parameters.AddWithValue("immatriculation", GetParam(ticket.id));
        command.Parameters.AddWithValue("fuel", GetParam(ticket.id));
        command.Parameters.AddWithValue("processed_file_name", GetParam(ticket.id));
        command.Parameters.AddWithValue("processed_file_size", GetParam(ticket.id));
        command.Parameters.AddWithValue("processed_user_name", GetParam(ticket.id));
        command.Parameters.AddWithValue("processed_user_id", GetParam(ticket.id));
        command.Parameters.AddWithValue("processed_date", GetParam(ticket.id));
        command.Parameters.AddWithValue("brand_code", GetParam(ticket.id));
        command.Parameters.AddWithValue("ecu_code", GetParam(ticket.id));
        command.Parameters.AddWithValue("brand_name", GetParam(ticket.id));
        command.Parameters.AddWithValue("dpf", GetParam(ticket.id));
        command.Parameters.AddWithValue("egr", GetParam(ticket.id));
        command.Parameters.AddWithValue("lambda", GetParam(ticket.id));
        command.Parameters.AddWithValue("hotstart", GetParam(ticket.id));
        command.Parameters.AddWithValue("flap", GetParam(ticket.id));
        command.Parameters.AddWithValue("adblue", GetParam(ticket.id));
        command.Parameters.AddWithValue("dtc", GetParam(ticket.id));
        command.Parameters.AddWithValue("torqmonitor", GetParam(ticket.id));
        command.Parameters.AddWithValue("speedlimit", GetParam(ticket.id));
        command.Parameters.AddWithValue("startstop", GetParam(ticket.id));
        command.Parameters.AddWithValue("nox", GetParam(ticket.id));
        command.Parameters.AddWithValue("tva", GetParam(ticket.id));
        command.Parameters.AddWithValue("readiness", GetParam(ticket.id));
        command.Parameters.AddWithValue("immo", GetParam(ticket.id));
        command.Parameters.AddWithValue("maf", GetParam(ticket.id));
        command.Parameters.AddWithValue("hardcut", GetParam(ticket.id));
        command.Parameters.AddWithValue("displaycalibration", GetParam(ticket.id));
        command.Parameters.AddWithValue("waterpump", GetParam(ticket.id));
        command.Parameters.AddWithValue("tprot", GetParam(ticket.id));
        command.Parameters.AddWithValue("o2", GetParam(ticket.id));
        command.Parameters.AddWithValue("glowplugs", GetParam(ticket.id));
        command.Parameters.AddWithValue("y75", GetParam(ticket.id));
        command.Parameters.AddWithValue("special", GetParam(ticket.id));
        command.Parameters.AddWithValue("decata", GetParam(ticket.id));
        command.Parameters.AddWithValue("vmax", GetParam(ticket.id));
        command.Parameters.AddWithValue("stage1", GetParam(ticket.id));
        command.Parameters.AddWithValue("stage2", GetParam(ticket.id));
        command.Parameters.AddWithValue("flexfuel", GetParam(ticket.id));
    }
    
    public List<Ticket> GetTickets()
    {
        List<Ticket> requestResult = new List<Ticket>();
        
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT * FROM ps_ticket";
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                Ticket current = new Ticket();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }

        }
        
        return requestResult;
    }

    public List<Ticket> GetTicketByTenant(string tenant)
    {
        List<Ticket> requestResult = new List<Ticket>();
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT * FROM ps_ticket WHERE tenant = @tenant";
            
            command.Parameters.AddWithValue("tenant", tenant);
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                Ticket current = new Ticket();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
        }

        return requestResult;
    }
    

    public void Create(Ticket ticket)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"INSERT INTO ps_user (id, tenant, level, user_id, user_name, date, " +
                                  $"file_name, file_size, immatriculation, fuel, " +
                                  $"processed_file_name, processed_file_size, processed_user_name, processed_user_id, processed_date," +
                                  $"brand_code, ecu_code, brand_name, dpf, egr, lambda, hotstart, flap, adblue, dtc, torqmonitor, speedlimit," +
                                  $"startstop, nox, tva, readiness, immo, maf, hardcut, displaycalibration, waterpump, tprot, o2, glowplugs," +
                                  $" y75, special, decata, vmax, stage1, stage2, flexfuel)" +
                                  $"VALUES (@id, @tenant, @level, @user_id, @user_name, @date, " +
                                  $"@file_name, @file_size, @immatriculation, @fuel, " +
                                  $"@processed_file_name, @processed_file_size, @processed_user_name, @processed_user_id, @processed_date," +
                                  $"@brand_code, @ecu_code, @brand_name, @dpf, @egr, @lambda, @hotstart, @flap, @adblue, @dtc, @torqmonitor, @speedlimit," +
                                  $"@startstop, @nox, @tva, @readiness, @immo, @maf, @hardcut, @displaycalibration, @waterpump, @tprot, @o2, @glowplugs," +
                                  $" @y75, @special, @decata, @vmax, @stage1, @stage2, @flexfuel)";
            AddInCommand(command, ticket);
            command.ExecuteNonQuery();
        }
    }
    
    public void Update(Ticket ticket)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = 
                "UPDATE ps_user" +
                $"SET (tenant = @tenant, level = @level, user_id = @user_id, user_name = @user_name, date = @date, " +
                $"file_name = @file_name, file_size = @file_size, immatriculation = @immatriculation, fuel = @fuel, " +
                $"processed_file_name = @processed_file_name, processed_file_size = @processed_file_size," +
                $"processed_user_name = @processed_user_name, processed_user_id = @processed_user_id, processed_date = @processed_date," +
                $"brand_code = @brand_code, ecu_code = @ecu_code, brand_name = @brand_name, dpf = @dpf, egr = @egr, lambda = @lambda," +
                $"hotstart = @hotstart, flap = @flap, adblue = @adblue, dtc = @dtc, torqmonitor = @torqmonitor, speedlimit = @speedlimit," +
                $"startstop = @startstop, nox = @nox, tva = @tva, readiness = @readiness, immo = @immo, maf = @maf, hardcut= @hardcut," +
                $"displaycalibration = @displaycalibration, waterpump = @waterpump, tprot = @tprot, o2 = @o2, glowplugs = @glowplugs," +
                $"y75 = @y75, special = @special, decata = @decata, vmax = @vmax, stage1 = @stage1, stage2 = @stage2, flexfuel = @flexfuel)," +
                $"WHERE id = @id;";
            
            AddInCommand(command, ticket);
            command.ExecuteNonQuery();
        }
        
    }
}