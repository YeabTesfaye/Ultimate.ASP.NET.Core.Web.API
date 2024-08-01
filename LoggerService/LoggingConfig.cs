using NLog;
using NLog.Config;
using NLog.Targets;

namespace LoggerService;
public static class LoggingConfig
{

    public static void Configure()
    {
        var config = new LoggingConfiguration();
        // Define the file target
        var logfile = new FileTarget("logfile")
        {
            FileName = "file.txt",
            Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}|${exception:format=tostring}"
        };
        config.AddTarget("logfile", logfile);
        // Define logging rules
        // Log warnings, errors, and fatal messages to the file
        config.AddRule(LogLevel.Warn, LogLevel.Fatal, logfile);

        LogManager.Configuration = config;
    }

}