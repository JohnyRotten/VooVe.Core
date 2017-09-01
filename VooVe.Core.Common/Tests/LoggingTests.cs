using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VooVe.Core.Common.Services.Logging;
using VooVe.Core.Common.SystemExtentions;

namespace VooVe.Core.Common.Tests
{
    [TestFixture]
    public class LoggingTests
    {

        private TestLogger _logger;
        private Dictionary<LoggingVerbosity, List<string>> _dictionary;

        [OneTimeSetUp]
        public void ClassInitialize()
        {
            _dictionary = new Dictionary<LoggingVerbosity, List<string>>
            {
                { LoggingVerbosity.Details, new List<string> { "Det1", "Det2" } },
                { LoggingVerbosity.Diagnostic, new List<string> { "Dia1", "Dia2" } },
                { LoggingVerbosity.Normal, new List<string> { "Nor1", "nor2" } },
                { LoggingVerbosity.Minimal, new List<string> { "Min1", "Min2" } }
            };
        }

        private void Log()
        {
            _dictionary.ForEach(pair =>
            {
                switch (pair.Key)
                {
                    case LoggingVerbosity.Minimal:
                        pair.Value.ForEach(l => _logger.Error(l));
                        break;
                    case LoggingVerbosity.Normal:
                        pair.Value.ForEach(l => _logger.Warning(l));
                        break;
                    case LoggingVerbosity.Diagnostic:
                        pair.Value.ForEach(l => _logger.Information(l));
                        break;
                    case LoggingVerbosity.Details:
                        pair.Value.ForEach(l => _logger.Debug(l));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        private Tuple<IEnumerable<string>, IEnumerable<string>> SplitBy(LoggingVerbosity verbosity)
        {
            var included = _dictionary.Where(p => p.Key <= verbosity).SelectMany(p => p.Value).ToList();
            var excluded = _dictionary.SelectMany(p => p.Value).Except(included);
            return new Tuple<IEnumerable<string>, IEnumerable<string>>(included, excluded);
        }

        [SetUp]
        public void TestInitialize()
        {
            _logger = new TestLogger();
        }

        [TestCase(LoggingVerbosity.Diagnostic)]
        [TestCase(LoggingVerbosity.Details)]
        [TestCase(LoggingVerbosity.Normal)]
        [TestCase(LoggingVerbosity.Minimal)]
        public void LoggingTest(LoggingVerbosity verbosity)
        {
            _logger.Verbosity = verbosity;
            Log();
            var splitted = SplitBy(_logger.Verbosity);
            splitted.Item1.ForEach(m => Assert.AreEqual(true, _logger.Contains(m)));
            splitted.Item2.ForEach(m => Assert.AreEqual(false, _logger.Contains(m)));
        }

        #region Logger Implementation

        public class TestLogger : Logger
        {
            public TestLogger()
            {
                PrintTime = false;
            }

            private readonly ICollection<string> _log = new List<string>();

            public bool Contains(string message) => _log.Contains(message);

            protected internal override void WriteLog(string message)
            {
                _log.Add(message);
            }
        }

        #endregion
    }
}