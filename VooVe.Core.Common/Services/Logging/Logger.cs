using System;

namespace VooVe.Core.Common.Services.Logging
{
    public abstract class Logger
    {
        public virtual LoggingVerbosity Verbosity { get; set; }

        public bool PrintTime { get; set; } = true;

        public void Message(string format, params object[] args) => Log(format, args);

        public void Error(string format, params object[] args) => Log(format, args);

        public void Debug(string format, params object[] args)
        {
            if ((Verbosity & LoggingVerbosity.Details) == LoggingVerbosity.Details)
            {
                Log(format, args);
            }
        }

        public void Information(string format, params object[] args)
        {
            if ((Verbosity & LoggingVerbosity.Details) == LoggingVerbosity.Details ||
                (Verbosity & LoggingVerbosity.Diagnostic) == LoggingVerbosity.Diagnostic)
            {
                Log(format, args);
            }
        }

        public void Warning(string format, params object[] args)
        {
            if ((Verbosity & LoggingVerbosity.Details) == LoggingVerbosity.Details ||
                (Verbosity & LoggingVerbosity.Diagnostic) == LoggingVerbosity.Diagnostic ||
                (Verbosity & LoggingVerbosity.Normal) == LoggingVerbosity.Normal)
            {
                Log(format, args);
            }
        }

        private void Log(string format, params object[] args) => 
            WriteLog(string.Format($"{(PrintTime?$"{DateTime.Now:u} ":string.Empty)}{format}", args));

        protected internal abstract void WriteLog(string message);
    }
}