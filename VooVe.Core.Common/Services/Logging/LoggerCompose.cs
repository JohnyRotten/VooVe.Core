using VooVe.Core.Common.SystemExtentions;

namespace VooVe.Core.Common.Services.Logging
{
    public class LoggerCompose : Logger
    {
        private readonly Logger[] _loggers;
        private LoggingVerbosity _verbosity;

        public override LoggingVerbosity Verbosity
        {
            get => _verbosity;
            set
            {
                _verbosity = value;
                _loggers.ForEach(l => l.Verbosity = value);
            }
        }

        public LoggerCompose(params Logger[] loggers)
        {
            _loggers = loggers;
        }

        protected internal override void WriteLog(string message)
        {
            _loggers.ForEach(l =>
            {
                l.WriteLog(message);
            });
        }
    }
}