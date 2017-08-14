using System;

namespace VooVe.Core.Services.Logging
{
    public class ConsoleLogger : Logger
    {
        protected internal override void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
    }
}