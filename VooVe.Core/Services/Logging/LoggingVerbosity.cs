using System;

namespace VooVe.Core.Services.Logging
{
    [Flags]
    public enum LoggingVerbosity
    {
        Minimal, Normal, Diagnostic, Details
    }
}