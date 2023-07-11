using log4net;
using log4net.Config;

namespace CSqlManager;

public class MyLogManager
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    private static bool isConfig = false;
    
    public static void Configure(string file = "log4net.config")
    {
        XmlConfigurator.Configure(new FileInfo(file));
        isConfig = true;
    }

    public static void Log(string message)
    {
        if (!isConfig)
        {
            Configure();
        }
        log.Info(message);
    }
    
    public static void Warn(string message)
    {
        if (!isConfig)
        {
            Configure();
        }
        log.Warn(message);
    }
    
    public static void Error(string message)
    {
        if (!isConfig)
        {
            Configure();
        }
        log.Error(message);
    }
}