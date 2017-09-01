using System.IO;

namespace VooVe.Core.Common.Services.Logging
{
    public class FileLogger : Logger
    {
        private readonly string _path;

        public FileLogger(string path)
        {
            _path = path;
        }

        protected internal override void WriteLog(string message)
        {
            using (var stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
            using (var writter = new StreamWriter(stream))
            {
                writter.WriteLine(message);
            }
        }
    }
}