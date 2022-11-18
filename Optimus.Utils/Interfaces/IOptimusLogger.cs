using Serilog.Events;

namespace Optimus.Utils
{
    public interface IOptimusLogger
    {
        void Log(string content, LogLevel logLevel = LogLevel.Error);
        void Log(string section, string content, LogLevel logLevel = LogLevel.Error);
    }

    public enum LogLevel
    {
        Error = LogEventLevel.Error,
        Warning = LogEventLevel.Warning,
        Information = LogEventLevel.Information,
        Fatal = LogEventLevel.Fatal,
        Debug = LogEventLevel.Debug
    }
}
