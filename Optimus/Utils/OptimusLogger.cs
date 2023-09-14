
namespace Optimus.Utils
{
    public class OptimusLogger : IOptimusLogger
    {
        public OptimusLogger()
        {
        }

        public OptimusLogger(string section, bool useTimeInLogFile = false)
        {
           
        }

        public void Log(string content, LogLevel logLevel = LogLevel.Error)
        {
        }

        public void Log(string section, string content, LogLevel logLevel = LogLevel.Error)
        {
        }
    }
}
