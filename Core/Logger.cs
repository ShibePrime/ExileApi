using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ExileCore
{
    public class Logger
    {
        private static ILogger _instance;
        public static ILogger Log =>
            _instance ?? (_instance = new LoggerConfiguration().MinimumLevel
                .ControlledBy(new LoggingLevelSwitch(LogEventLevel.Verbose)).WriteTo
                .Logger(l => l.Filter.ByIncludingOnly(
                        e => e.Level == LogEventLevel.Information).WriteTo
                    .File(@"Logs\Info-.log", rollingInterval: RollingInterval.Day)).WriteTo
                .Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                    .WriteTo.File(@"Logs\Debug-.log", rollingInterval: RollingInterval.Day)).WriteTo
                .Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                    .WriteTo.File(@"Logs\Warning-.log", rollingInterval: RollingInterval.Day)).WriteTo
                .Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(@"Logs\Error-.log", rollingInterval: RollingInterval.Day)).WriteTo
                .Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
                    .WriteTo.File(@"Logs\Fatal-.log", rollingInterval: RollingInterval.Day)).WriteTo
                .File(@"Logs\Verbose-.log", rollingInterval: RollingInterval.Day).CreateLogger());
    }
}
