using System.Collections.Generic;
using Serilog;

namespace sdLitica.IntegrationTests.TestUtils.BddUtils
{
    public class GivenStatement : BddStatement
    {
        private IDictionary<string, object> _givenData = new Dictionary<string, object>();

        public GivenStatement(ILogger logger) : base(logger)
        {
        }

        public WhenStatement When => new WhenStatement(Logger, _givenData);

        public void AddData<T>(T data, string additionalKey = null)
        {
            _givenData.Add(GetDictionaryKey(typeof(T), additionalKey), data);
        }
    }
}