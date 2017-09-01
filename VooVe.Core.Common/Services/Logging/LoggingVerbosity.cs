using System;

namespace VooVe.Core.Common.Services.Logging
{
    [Flags]
    public enum LoggingVerbosity
    {
        Minimal, Normal, Diagnostic, Details
    }
}