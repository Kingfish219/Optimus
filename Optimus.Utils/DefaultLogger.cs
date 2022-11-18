using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Optimus.Utils
{
    public class DefaultLogger : IOptimusLogger
    {
        private readonly Logger _logger;

        public DefaultLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File($@"logs\{DateTime.Now:yyyy-MM-dd}\{DateTime.Now:yyyy-MM-dd}.txt")
                .CreateLogger();
        }

        public DefaultLogger(string section, bool useTimeInLogFile = false)
        {
            if(useTimeInLogFile)
            {
                _logger = new LoggerConfiguration()
                .WriteTo.File($@"logs\{section}\{DateTime.Now:yyyy-MM-dd}\{DateTime.Now:yyyy-MM-dd}_{DateTime.Now:HH-mm-ss}.txt")
                .CreateLogger();
            }
            else
            {
                _logger = new LoggerConfiguration()
                .WriteTo.File($@"logs\{section}\{DateTime.Now:yyyy-MM-dd}\{DateTime.Now:yyyy-MM-dd}.txt")
                .CreateLogger();
            }
        }

        public void Log(string content, LogLevel logLevel = LogLevel.Error)
        {
            Serilog.Log.CloseAndFlush();
            //var logger = new LoggerConfiguration()
            //    .WriteTo.File($@"logs\{DateTime.Now:yyyy-MM-dd}\{DateTime.Now:yyyy-MM-dd}.txt")
            //    .CreateLogger();

            _logger.Write((LogEventLevel)logLevel, content);
        }

        public void Log(string section, string content, LogLevel logLevel = LogLevel.Error)
        {
            Serilog.Log.CloseAndFlush();
            //var logger = new LoggerConfiguration()
            //    .WriteTo.File($@"logs\{section}\{DateTime.Now:yyyy-MM-dd}\{DateTime.Now:yyyy-MM-dd}.txt")
            //    .CreateLogger();

            _logger.Write((LogEventLevel)logLevel, $"[{section}] " + content);
        }
    }
}
