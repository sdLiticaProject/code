using System;
using Serilog;

namespace sdLitica.IntegrationTests.TestUtils.BddUtils
{
    public class BddStatement
    {
        protected readonly ILogger Logger;

        public BddStatement(ILogger logger)
        {
            Logger = logger;
        }

        public string GetDictionaryKey(Type dataType, string additionalKey)
        {
            return $"{additionalKey}_{dataType.Name}";
        }

        public ILogger GetStatementLogger()
        {
            return Logger;
        }
    }
}