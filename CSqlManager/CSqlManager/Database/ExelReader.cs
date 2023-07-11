
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
            MyLogManager.Log("Exel document read");
        }
    }
    
    public void LinkWithDatabase()
    {
        string connString = DbAccess.connString;
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
                MyLogManager.Log("ps_brand fields updated");
                // Set up ps_ecu fields 
                foreach (var ecu in _ecus)
                {
                    bool goNext = true;
                    command.CommandText = $"SELECT code, brand_code FROM ps_ecu WHERE code = @Ecu AND brand_code = @Code";
                        
                    if(command.Parameters.Contains("Ecu"))
                        command.Parameters.Remove("Ecu");
                            
                    command.Parameters.AddWithValue("Ecu", ecu.code);
                    
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
                                              $"VALUES ('{ecu.Brand_code}', '{ecu.code}', '{ecu.Fuel}'," +
                                              $" {ecu.dpf.ToString().ToLower()}, {ecu.egr.ToString().ToLower()}," +
                                              $"{ecu.lambda.ToString().ToLower()}, {ecu.hotstart.ToString().ToLower()}," +
                                              $"{ecu.flap.ToString().ToLower()}, {ecu.adblue.ToString().ToLower()}," +
                                              $"{ecu.dtc.ToString().ToLower()}, {ecu.torqmonitor.ToString().ToLower()}," +
                                              $"{ecu.speedlimit.ToString().ToLower()}, {ecu.startstop.ToString().ToLower()}," +
                                              $"{ecu.nox.ToString().ToLower()}, {ecu.tva.ToString().ToLower()}," +
                                              $"{ecu.readiness.ToString().ToLower()}, {ecu.immo.ToString().ToLower()}," +
                                              $"{ecu.maf.ToString().ToLower()}, {ecu.hardcut.ToString().ToLower()}," +
                                              $"{ecu.displaycalibration.ToString().ToLower()}, {ecu.waterpump.ToString().ToLower()}," +
                                              $"{ecu.tprot.ToString().ToLower()}, {ecu.o2.ToString().ToLower()}," +
                                              $"{ecu.glowplugs.ToString().ToLower()}, {ecu.y75.ToString().ToLower()}," +
                                              $"{ecu.special.ToString().ToLower()}, {ecu.decata.ToString().ToLower()}," +
                                              $"{ecu.vmax.ToString().ToLower()}, {ecu.stage1.ToString().ToLower()}," +
                                              $"{ecu.stage2.ToString().ToLower()}, {ecu.flexfuel.ToString().ToLower()})";
                        command.ExecuteNonQuery();
                    }
                    
                }       
                MyLogManager.Log("ps_ecu fields updated");
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

