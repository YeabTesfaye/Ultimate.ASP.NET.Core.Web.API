using NLog;
using NLog.Config;
using NLog.Targets;

namespace LoggerService;
public static class LoggingConfig
{

    public static void Configure()
    {
        var config = new LoggingConfiguration();

        var logfile = new FileTarget("logfile") { FileName = "file.txt" };
        var logconsole = new ConsoleTarget("logconsole");

        config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

        LogManager.Configuration = config;
    }

}