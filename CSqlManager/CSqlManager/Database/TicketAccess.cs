using Npgsql;
namespace CSqlManager;

public class TicketAccess : DbAccess
{
    private void AddFromReader(NpgsqlDataReader reader, Ticket ticket)
    {
        ticket.id = (int)getInt(reader, "id", true);
        ticket.tenant = getString(reader, "tenant");
        ticket.level = getString(reader, "level");
        ticket.user_id = getInt(reader, "user_id");
        ticket.user_name = getString(reader, "user_name");
        ticket.date = getDateTime(reader, "date");
        ticket.file_name = getString(reader, "file_name");
        ticket.file_id = getString(reader, "file_id");
        ticket.file_size = getInt(reader, "file_size");
        ticket.immatriculation = getString(reader, "immatriculation");
        ticket.fuel = getString(reader, "fuel");
        
        ticket.processed_file_name = getString(reader, "processed_file_name");
        ticket.processed_file_size = getInt(reader, "processed_file_size");
        ticket.processed_user_name = getString(reader, "processed_user_name");
        ticket.comment = getString(reader, "comment");
        ticket.processed_user_id = getInt(reader, "processed_user_id");
        ticket.processed_date = getDateTime(reader, "processed_date");
        
        ticket.brand_code = getString(reader, "brand_code");
        ticket.ecu_code = getString(reader, "ecu_code");
        ticket.brand_name = getString(reader, "brand_name");
    
        ticket.dpf = (bool)getBoolean(reader, "dpf", false)!;
        ticket.egr = (bool)getBoolean(reader, "egr", false)!;
        ticket.lambda = (bool)getBoolean(reader, "lambda", false)!;
        ticket.hotstart = (bool)getBoolean(reader, "hotstart", false)!;
        ticket.flap = (bool)getBoolean(reader, "flap", false)!;
        ticket.dtc = (bool)getBoolean(reader, "dtc", false)!;
        ticket.adblue = (bool)getBoolean(reader, "adblue", false)!;
        ticket.torqmonitor = (bool)getBoolean(reader, "torqmonitor", false)!;
        ticket.speedlimit = (bool)getBoolean(reader, "speedlimit", false)!;
        ticket.startstop = (bool)getBoolean(reader, "startstop", false)!;
        ticket.nox = (bool)getBoolean(reader, "nox", false)!;
        ticket.tva = (bool)getBoolean(reader, "tva", false)!;
        ticket.readiness = (bool)getBoolean(reader, "readiness", false)!;
        ticket.immo = (bool)getBoolean(reader, "immo", false)!;
        ticket.maf = (bool)getBoolean(reader, "maf", false)!;
        ticket.hardcut = (bool)getBoolean(reader, "hardcut", false)!;
        ticket.displaycalibration = (bool)getBoolean(reader, "displaycalibration", false)!;
        ticket.waterpump = (bool)getBoolean(reader, "waterpump", false)!;
        ticket.tprot = (bool)getBoolean(reader, "tprot", false)!;
        ticket.o2 = (bool)getBoolean(reader, "o2", false)!;
        ticket.glowplugs = (bool)getBoolean(reader, "glowplugs", false)!;
        ticket.y75 = (bool)getBoolean(reader, "y75", false)!;
        ticket.special = (bool)getBoolean(reader, "special", false)!;
        ticket.decata = (bool)getBoolean(reader, "decata", false)!;
        ticket.vmax = (bool)getBoolean(reader, "vmax", false)!;
        ticket.stage1 = (bool)getBoolean(reader, "stage1", false)!;
        ticket.stage2 = (bool)getBoolean(reader, "stage2", false)!;
        ticket.flexfuel = (bool)getBoolean(reader, "flexfuel", false)!;
        
    }

    private void AddInCommand(NpgsqlCommand command, Ticket ticket)
    {
        command.Parameters.AddWithValue("tenant", GetParam(ticket.tenant));
        command.Parameters.AddWithValue("level", GetParam(ticket.level));
        command.Parameters.AddWithValue("user_id", GetParam(ticket.user_id));
        command.Parameters.AddWithValue("user_name", GetParam(ticket.user_name));
        command.Parameters.AddWithValue("date", GetParam(ticket.date));
        command.Parameters.AddWithValue("file_name", GetParam(ticket.file_name));
        command.Parameters.AddWithValue("file_id", GetParam(ticket.file_id));
        command.Parameters.AddWithValue("file_size", GetParam(ticket.file_size));
        command.Parameters.AddWithValue("immatriculation", GetParam(ticket.immatriculation));
        command.Parameters.AddWithValue("fuel", GetParam(ticket.fuel));
        command.Parameters.AddWithValue("processed_file_name", GetParam(ticket.processed_file_name));
        command.Parameters.AddWithValue("processed_file_size", GetParam(ticket.processed_file_size));
        command.Parameters.AddWithValue("processed_user_name", GetParam(ticket.processed_user_name));
        command.Parameters.AddWithValue("comment", GetParam(ticket.comment));
        command.Parameters.AddWithValue("processed_user_id", GetParam(ticket.processed_user_id));
        command.Parameters.AddWithValue("processed_date", GetParam(ticket.processed_date));
        command.Parameters.AddWithValue("brand_code", GetParam(ticket.brand_code));
        command.Parameters.AddWithValue("ecu_code", GetParam(ticket.ecu_code));
        command.Parameters.AddWithValue("brand_name", GetParam(ticket.brand_name));
        command.Parameters.AddWithValue("dpf", GetParam(ticket.dpf));
        command.Parameters.AddWithValue("egr", GetParam(ticket.egr));
        command.Parameters.AddWithValue("lambda", GetParam(ticket.lambda));
        command.Parameters.AddWithValue("hotstart", GetParam(ticket.hotstart));
        command.Parameters.AddWithValue("flap", GetParam(ticket.flap));
        command.Parameters.AddWithValue("adblue", GetParam(ticket.adblue));
        command.Parameters.AddWithValue("dtc", GetParam(ticket.dtc));
        command.Parameters.AddWithValue("torqmonitor", GetParam(ticket.torqmonitor));
        command.Parameters.AddWithValue("speedlimit", GetParam(ticket.torqmonitor));
        command.Parameters.AddWithValue("startstop", GetParam(ticket.startstop));
        command.Parameters.AddWithValue("nox", GetParam(ticket.nox));
        command.Parameters.AddWithValue("tva", GetParam(ticket.tva));
        command.Parameters.AddWithValue("readiness", GetParam(ticket.readiness));
        command.Parameters.AddWithValue("immo", GetParam(ticket.immo));
        command.Parameters.AddWithValue("maf", GetParam(ticket.maf));
        command.Parameters.AddWithValue("hardcut", GetParam(ticket.hardcut));
        command.Parameters.AddWithValue("displaycalibration", GetParam(ticket.displaycalibration));
        command.Parameters.AddWithValue("waterpump", GetParam(ticket.waterpump));
        command.Parameters.AddWithValue("tprot", GetParam(ticket.tprot));
        command.Parameters.AddWithValue("o2", GetParam(ticket.o2));
        command.Parameters.AddWithValue("glowplugs", GetParam(ticket.glowplugs));
        command.Parameters.AddWithValue("y75", GetParam(ticket.y75));
        command.Parameters.AddWithValue("special", GetParam(ticket.special));
        command.Parameters.AddWithValue("decata", GetParam(ticket.decata));
        command.Parameters.AddWithValue("vmax", GetParam(ticket.vmax));
        command.Parameters.AddWithValue("stage1", GetParam(ticket.stage1));
        command.Parameters.AddWithValue("stage2", GetParam(ticket.stage2));
        command.Parameters.AddWithValue("flexfuel", GetParam(ticket.flexfuel));
    }
    
    public List<Ticket> GetTickets()
    {
        List<Ticket> requestResult = new List<Ticket>();
        
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT * FROM ps_ticket ORDER BY id DESC";
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                Ticket current = new Ticket();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
            Close(Connection);
        }
        
        return requestResult;
    }

    public List<Ticket> GetInProgress(int userId)
    {
        List<Ticket> requestResult = new List<Ticket>();
        
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT * FROM ps_ticket where processed_file_name is NULL and (processed_user_id IS NULL OR processed_user_id = 0 OR processed_user_id = @userId) ORDER BY CASE level " +
            $" WHEN 'Platine' THEN 1 WHEN 'Gold' THEN 2 WHEN 'Silver' THEN 3 ELSE 4 END, id DESC";
            command.Parameters.AddWithValue("userId", userId);
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                Ticket current = new Ticket();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
            Close(Connection);
        }
        
        return requestResult;
    }
    public List<Ticket> GetByTenant(string tenant)
    {
        List<Ticket> requestResult = new List<Ticket>();
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT * FROM ps_ticket WHERE tenant = @tenant ORDER BY id DESC";
            
            command.Parameters.AddWithValue("tenant", tenant);
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                Ticket current = new Ticket();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
            Close(Connection);
        }

        return requestResult;
    }

    public Ticket? GetById(int id)
    {
        Ticket? requestResult = null;
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT * FROM ps_ticket WHERE id = @id";
            
            command.Parameters.AddWithValue("id", id);
            var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                requestResult = new Ticket();
                AddFromReader(reader, requestResult);
            }
            Close(Connection);
        }

        return requestResult;
    }
    

    public void Create(Ticket ticket)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"INSERT INTO ps_ticket (id, tenant, level, user_id, user_name, date, " +
                                  $"file_name, file_id, file_size, immatriculation, fuel, " +
                                  $"processed_file_name, processed_file_size, processed_user_name, comment, processed_user_id, processed_date," +
                                  $"brand_code, ecu_code, brand_name, dpf, egr, lambda, hotstart, flap, adblue, dtc, torqmonitor, speedlimit," +
                                  $"startstop, nox, tva, readiness, immo, maf, hardcut, displaycalibration, waterpump, tprot, o2, glowplugs," +
                                  $" y75, special, decata, vmax, stage1, stage2, flexfuel)" +
                                  $" VALUES (nextval('ps_ticket_id_seq'), @tenant, @level, @user_id, @user_name, @date, " +
                                  $"@file_name, @file_id, @file_size, @immatriculation, @fuel, " +
                                  $"@processed_file_name, @processed_file_size, @processed_user_name, @comment, @processed_user_id, @processed_date," +
                                  $"@brand_code, @ecu_code, @brand_name, @dpf, @egr, @lambda, @hotstart, @flap, @adblue, @dtc, @torqmonitor, @speedlimit," +
                                  $"@startstop, @nox, @tva, @readiness, @immo, @maf, @hardcut, @displaycalibration, @waterpump, @tprot, @o2, @glowplugs," +
                                  $" @y75, @special, @decata, @vmax, @stage1, @stage2, @flexfuel)";
            AddInCommand(command, ticket);
            command.ExecuteNonQuery();
            Close(Connection);
        }
    }
    
    public void Update(Ticket ticket)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = 
                "UPDATE ps_ticket" +
                $" SET tenant = @tenant, level = @level, user_id = @user_id, user_name = @user_name, date = @date, " +
                $"file_name = @file_name, file_size = @file_size, immatriculation = @immatriculation, fuel = @fuel, " +
                $"processed_file_name = @processed_file_name, processed_file_size = @processed_file_size," +
                $"processed_user_name = @processed_user_name, comment = @comment, processed_user_id = @processed_user_id, processed_date = @processed_date," +
                $"brand_code = @brand_code, ecu_code = @ecu_code, brand_name = @brand_name, dpf = @dpf, egr = @egr, lambda = @lambda," +
                $"hotstart = @hotstart, flap = @flap, adblue = @adblue, dtc = @dtc, torqmonitor = @torqmonitor, speedlimit = @speedlimit," +
                $"startstop = @startstop, nox = @nox, tva = @tva, readiness = @readiness, immo = @immo, maf = @maf, hardcut= @hardcut," +
                $"displaycalibration = @displaycalibration, waterpump = @waterpump, tprot = @tprot, o2 = @o2, glowplugs = @glowplugs," +
                $"y75 = @y75, special = @special, decata = @decata, vmax = @vmax, stage1 = @stage1, stage2 = @stage2, flexfuel = @flexfuel " +
                $" WHERE id = @id";
        
            MyLogManager.Debug($"command.CommandText: {command.CommandText}");
        
            AddInCommand(command, ticket);
            command.Parameters.AddWithValue("id", ticket.id);
            MyLogManager.Debug($"command.CommandText 2: {command}");
            command.ExecuteNonQuery();
            Close(Connection);
        }
        
    }
}