using Serilog;

namespace ApiGateway.Package.Logging
{
    public class Logger
    {
        private readonly Serilog.Core.Logger _logger;

        public Logger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File("logs.txt")
                .CreateLogger();
        }

        public void Log(string message)
        {
            _logger.Information($"{message}\n");
        }

    }

    public static class AppLogEvents
    {
        internal const int Create = 1000;
        internal const int Read = 1001;
        internal const int Update = 1002;
        internal const int Delete = 1003;

        internal const int Details = 3000;
        internal const int Error = 3001;

        internal const int ReadNotFound = 4000;
        internal const int UpdateNotFound = 4001;
    }
}
