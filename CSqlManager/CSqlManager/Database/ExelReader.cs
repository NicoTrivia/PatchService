
using OfficeOpenXml;
using Npgsql;

namespace CSqlManager;



public class ExelReader
{
    private string _path; // the .xlsx file path
    private List<Brand> _brands;
    private List<ECU> _ecus;

    public enum Table
    {
        brand,
        ecu
    }
    
    public ExelReader(string path)
    {
        _path = path;
        _brands = new List<Brand>();
        _ecus = new List<ECU>();
    }

    public void ExtractExel()
    {
        FileInfo fileInfo = new FileInfo(_path);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                string code = ToletterCode(worksheet.Name);
                _brands.Add(new Brand(code, worksheet.Name[0] + worksheet.Name.Substring(1).ToLower()));
                
                int rowCount = worksheet.Dimension.Rows;
                
                for (int row = 2; row <= rowCount; row++)
                {
                    var value = worksheet.Cells[row, 2].Value;
                    if (value != null)
                    {
                        bool[] annexes = new bool[28];
                        for (int i = 3; i < 31; i++)
                        {
                            annexes[i-3] = worksheet.Cells[row, i].Value != null;
                        }
                        
                        var fuel = worksheet.Cells[row, 1].Value;
                        if(fuel != null)
                            _ecus.Add(new ECU(code, value.ToString()!, fuel.ToString()!, annexes));
                        else
                            _ecus.Add(new ECU(code, value.ToString()!, annexes));
                    }
                }
            }
            Console.WriteLine("Exel document read");
        }
    }

    public void ShowInformations(string? brandrestriction = null)
    {
        Console.Write($"Brands {_brands.Count}: (");
        foreach (var brand in _brands)
        {
            if (brandrestriction != null )
            {
                if( brand.Code == brandrestriction.ToLower())
                    Console.Write(brand + ",  ");
            }
            else
            {
                Console.Write(brand + ",  ");
            }
        }
        Console.WriteLine(")");
        Console.WriteLine();
        Console.Write($"ECUs {_ecus.Count}: (");
        foreach (var ecu in _ecus)
        {
            if (brandrestriction != null)
            {
                if(ecu.Brand_code == brandrestriction.ToLower())
                    Console.Write(ecu + ",  ");
            }
            else
            {
                Console.Write(ecu + ",  ");
            }
        }
        Console.WriteLine(")");
    }

    public void CompareWithDatabase()
    {
        string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";
        using (NpgsqlConnection connection = new NpgsqlConnection(connString))
        {
            connection.Open();

            using (NpgsqlCommand command = connection.CreateCommand())
            {
                List<string> request1 = new List<string>(); 
                command.CommandText = $"SELECT code FROM ps_brand";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    request1.Add(reader.GetString(0));
                }
                reader.Close();
                Console.WriteLine($"Brands // in Database : {request1.Count}, in Local {_brands.Count}");
                if (request1.Count != _brands.Count)
                {
                    foreach(Brand brand in _brands)
                    {
                        if (!request1.Contains(brand.Code))
                        {
                            Console.WriteLine("differs :" +brand.Code);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No differs found");
                }
                
                
                List<(string, string)> request2 = new List<(string,string)>(); 
                command.CommandText = $"SELECT brand_code, code FROM ps_ecu";
                var reader2 = command.ExecuteReader();
                while (reader2.Read())
                {
                    var add = (reader.GetString(0), reader.GetString(1));
                    request2.Add(add);
                }
                reader.Close();
                Console.WriteLine($"Ecus // in Database : {request2.Count}, in Local {_ecus.Count}");
                if (request2.Count != _ecus.Count)
                {
                    foreach(ECU ecu in _ecus)
                    {
                        if (!request2.Contains((ecu.Brand_code, ecu.ECU_code)))
                        {
                            Console.WriteLine($"differs :{ecu.Brand_code}, {ecu.ECU_code}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No differs found");
                }
                
            }
        }
    }

    public List<string> SelectInEcu<T>(string name)
    {
        List<string> requestResult = new List<string>();
        string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";
        using (NpgsqlConnection connection = new NpgsqlConnection(connString))
        {
            connection.Open();

            using (NpgsqlCommand command = connection.CreateCommand())
            {

                command.CommandText = $"SELECT {name} FROM ps_ecu";
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if(typeof(T) == typeof(string))
                        requestResult.Add(reader.GetString(0));
                    if(typeof(T) == typeof(bool))
                        requestResult.Add(reader.GetBoolean(0).ToString());
                }

            }
        }

        return requestResult;
    }
    
    

    public void ResetDatabase()
    { 
        string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";
        using (NpgsqlConnection connection = new NpgsqlConnection(connString))
        {
            connection.Open();
            
            using (NpgsqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM ps_ecu";
                command.ExecuteNonQuery();
               
                command.CommandText = "DELETE FROM ps_brand";
                command.ExecuteNonQuery();
                Console.WriteLine("Database content deleted");
            }
            
            connection.Close();
        }
    }

    public void RemoveCol(Table table, string name)
    {
        string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";
        using (NpgsqlConnection connection = new NpgsqlConnection(connString))
        {
            connection.Open();
            
            using (NpgsqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"ALTER TABLE ps_{table} DROP COLUMN {name}";
                command.ExecuteNonQuery();
                
                Console.WriteLine($"Column {name} deleted from {table}");
            }
            
            connection.Close();
        }
    }

    public void AddVarChrCol(Table table, string name)
    {
        string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";
        using (NpgsqlConnection connection = new NpgsqlConnection(connString))
        {
            connection.Open();
            
            using (NpgsqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"ALTER TABLE ps_{table} ADD {name} varchar";
                command.ExecuteNonQuery();
                
                Console.WriteLine($"Column {name} created in {table}");
            }
            
            connection.Close();
        }
    }
    

    public void LinkWithDatabase()
    {
        string connString = "Server=dev.triviatech.fr;Port=5432;Database=patch_services;User Id=patch_admin;Password=alvira2023!;";
        using (NpgsqlConnection connection = new NpgsqlConnection(connString))
        {
            connection.Open();

            using (NpgsqlCommand command = connection.CreateCommand())
            {
                
                // Set up ps_brand fields 

                foreach (var brand in _brands)
                {
                    bool goNext = true;
                    command.CommandText = $"SELECT code FROM ps_brand WHERE code = @Code";

                    if (command.Parameters.Contains("Code"))
                        command.Parameters.Remove("Code");

                    command.Parameters.AddWithValue("Code", brand.Code);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        goNext = false;
                    }

                    reader.Close();
                    if (goNext)
                    {
                        command.CommandText = $"INSERT INTO ps_brand (code, name) VALUES ('{brand.Code}', '{brand.Name}')";
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("ps_brand fields updated");
                // Set up ps_ecu fields 
                foreach (var ecu in _ecus)
                {
                    bool goNext = true;
                    command.CommandText = $"SELECT code, brand_code FROM ps_ecu WHERE code = @Ecu AND brand_code = @Code";
                        
                    if(command.Parameters.Contains("Ecu"))
                        command.Parameters.Remove("Ecu");
                            
                    command.Parameters.AddWithValue("Ecu", ecu.ECU_code);
                    
                    if (command.Parameters.Contains("Code"))
                        command.Parameters.Remove("Code");

                    command.Parameters.AddWithValue("Code", ecu.Brand_code);
                    
                    var reader = command.ExecuteReader();
                   
                    while (reader.Read())
                    {
                        goNext = false;
                    }
                            
                    reader.Close();
                    if (goNext)
                    {
                        command.CommandText = $"INSERT INTO ps_ecu (brand_code, code, carburant,dpf, egr," +
                                              $" lambda, hotstart, flap, adblue, dtc, torqmonitor," +
                                              $" speedlimit, startstop, nox, tva, readiness, immo," +
                                              $" maf, hardcut, displaycalibration, waterpump, tprot," +
                                              $" o2, glowplugs, y75, special, decata, vmax, stage1," +
                                              $" stage2, flexfuel) " +
                                              $"VALUES ('{ecu.Brand_code}', '{ecu.ECU_code}', '{ecu.Fuel}'," +
                                              $" {ecu.toggles[0].ToString().ToLower()}, {ecu.toggles[1].ToString().ToLower()}," +
                                              $"{ecu.toggles[2].ToString().ToLower()}, {ecu.toggles[3].ToString().ToLower()}," +
                                              $"{ecu.toggles[4].ToString().ToLower()}, {ecu.toggles[5].ToString().ToLower()}," +
                                              $"{ecu.toggles[6].ToString().ToLower()}, {ecu.toggles[7].ToString().ToLower()}," +
                                              $"{ecu.toggles[8].ToString().ToLower()}, {ecu.toggles[9].ToString().ToLower()}," +
                                              $"{ecu.toggles[10].ToString().ToLower()}, {ecu.toggles[11].ToString().ToLower()}," +
                                              $"{ecu.toggles[12].ToString().ToLower()}, {ecu.toggles[13].ToString().ToLower()}," +
                                              $"{ecu.toggles[14].ToString().ToLower()}, {ecu.toggles[15].ToString().ToLower()}," +
                                              $"{ecu.toggles[16].ToString().ToLower()}, {ecu.toggles[17].ToString().ToLower()}," +
                                              $"{ecu.toggles[18].ToString().ToLower()}, {ecu.toggles[19].ToString().ToLower()}," +
                                              $"{ecu.toggles[20].ToString().ToLower()}, {ecu.toggles[21].ToString().ToLower()}," +
                                              $"{ecu.toggles[22].ToString().ToLower()}, {ecu.toggles[23].ToString().ToLower()}," +
                                              $"{ecu.toggles[24].ToString().ToLower()}, {ecu.toggles[25].ToString().ToLower()}," +
                                              $"{ecu.toggles[26].ToString().ToLower()}, {ecu.toggles[27].ToString().ToLower()})";
                        command.ExecuteNonQuery();
                    }
                    
                }       
                Console.WriteLine("ps_ecu fields updated");
            }
            connection.Close();
        }
    }

    private string ToletterCode(string input)
    {
        return input.Replace(' ', '_').Replace('\'', '_').ToUpper();
    }


    private void Print(ExcelWorksheet worksheet)
    {
        int rowCount = worksheet.Dimension.Rows;
        int colCount = worksheet.Dimension.Columns;

                
        bool isInLine = false;
        for (int row = 1; row <= rowCount; row++)
        {
            for (int col = 1; col <= colCount; col++)
            { 
                var value = worksheet.Cells[row, col].Value;
                if (value != null)
                {
                    isInLine = true;
                    Console.Write(value + "\t");
                }
            }

            if (isInLine)
            { 
                isInLine = false;
                Console.WriteLine();
            }
        }
    }
    
}

